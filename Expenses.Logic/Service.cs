using System;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Expenses.Data;
using FluentValidation;

namespace Expenses.Logic
{
    public abstract class Service<T> where T : class
    {
        private readonly ExpensesContext _context;
        protected DbSet<T> DbSet { get; }

        protected Service()
        {
            _context = new ExpensesContext();
            DbSet = _context.Set<T>();
        }

        // CRUD Logic

        public virtual ObservableCollection<T> Get()
        {
            DbSet.Load();
            return DbSet.Local;
        }

        public virtual ObservableCollection<T> Get(Expression<Func<T, bool>> filter)
        {
            var result = DbSet.Where(filter);
            return new ObservableCollection<T>(result.ToList());
        }

        public virtual ObservableCollection<T> Get(Expression<Func<T, bool>> filter, 
                                            params Expression<Func<T, object>>[] includes)
        {
            var result = DbSet.Where(filter);
            result = includes.Aggregate(result, (current, include) => current.Include(include));
            return new ObservableCollection<T>(result.ToList());
        }

        public virtual T One(Expression<Func<T, bool>> predecate)
        {
            return DbSet.SingleOrDefault(predecate);
        }

        public virtual T Find(params object[] key)
        {
            return DbSet.Find(key);
        }

        public virtual void Delete(T entity)
        {
            DbSet.Remove(entity);
            _context.SaveChanges();
        }

        public virtual void Reload(T entity)
        {
            if (entity == null || State(entity) == EntityState.Detached) return;
            _context.Entry(entity).Reload();
        }

        protected int Save()
        {
            return _context.SaveChanges();
        }

        protected virtual void Save(T entity, AbstractValidator<T> validator)
        {
            var result = validator.Validate(entity);
            if(result.IsValid)
            {
                _context.SaveChanges();
            }
            else
            {
                var message = result.Errors
                    .Aggregate("", (c, r) => c + r.ErrorMessage + "\n");
                throw new ValidationException(message, result.Errors);
            }
        }

        //public decimal Sum(Expression<Func<T, decimal>> selector)
        //{
        //    return DbSet.Sum(selector);
        //}
        
        // States logic

        public virtual EntityState State(T entity)
        {
            return entity == null 
                ? EntityState.Detached 
                : _context.Entry(entity).State;
        }

        public virtual bool IsDirty(T entity)
        {
            return entity != null && (State(entity) == EntityState.Added ||
                                      State(entity) == EntityState.Modified);
        }

        public virtual void SetAdded(T entity)
        {
            if (entity != null && State(entity) == EntityState.Detached)
                DbSet.Add(entity);
        }

        public virtual void SetModified(T entity)
        {
            if (entity != null && State(entity) != EntityState.Modified)
                _context.Entry(entity).State = EntityState.Modified;
        }

        public virtual T Attach(T entity)
        {
            return entity == null ? null : DbSet.Attach(entity);
        }
    }
}