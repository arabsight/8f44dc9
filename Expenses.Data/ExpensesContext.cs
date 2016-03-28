using System;
using System.Data.Entity;
using System.Linq;
using Expenses.Core;
using Expenses.Core.Shared;
using Expenses.Data.Mappings;

namespace Expenses.Data
{
    public class ExpensesContext : DbContext
    {
        public ExpensesContext() : base(DbHelper.SqlCeConnection, true)
        {
            Configuration.LazyLoadingEnabled = false;
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Exercise> Exercises { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ReceiptType> ReceiptTypes { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<Withdrawal> Withdrawals { get; set; }

        protected override void OnModelCreating(DbModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Configurations.Add(new UserMapping());
            builder.Configurations.Add(new CategoryMapping());
            builder.Configurations.Add(new ReceiptTypeMapping());
            builder.Configurations.Add(new ExerciseMapping());
            builder.Configurations.Add(new ExpenseMapping());
            builder.Configurations.Add(new WithdrawalMapping());
        }

        public override int SaveChanges()
        {
            SetTimestamps();

            return base.SaveChanges();
        }

        private void SetTimestamps()
        {
            var exercises = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

            foreach (var entry in exercises)
            {
                var entity = entry.Entity as ITrackable;
                if (entity == null) continue;

                if (entry.State == EntityState.Added)
                {
                    entity.CreatedAt = DateTime.Now;
                    entity.UpdatedAt = DateTime.Now;
                }
                else
                    entity.UpdatedAt = DateTime.Now;
            }
        }
    }
}