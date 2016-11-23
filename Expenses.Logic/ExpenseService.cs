using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using Expenses.Core;
using Expenses.Core.Helpers;
using Expenses.Logic.Validation;

namespace Expenses.Logic
{
    public class ExpenseService : Service<Expense>
    {
        public ExpenseService() : base(ExpenseValidator.Default) {}

        public static ExpenseService Instance => new ExpenseService();

        public ObservableCollection<Expense> GetByExercise(int exercise)
        {
            return Get(e => e.ExerciseId == exercise, e => e.Category, e => e.ReceiptType);
        }

        public decimal GetTotalByExercise(Exercise exercise)
        {
            var query = DbSet.Where(e => e.ExerciseId == exercise.Id);
            return query.Any() ? query.Sum(e => e.Amount) : 0;
        }

        public IEnumerable<ExpenseTotal> GetMonthlyTotalsByCategory(Exercise exercise)
        {
            var result = DbSet
                .Include(e => e.Category)
                .Include(e => e.Exercise)
                .Where(e => e.ExerciseId == exercise.Id)
                .GroupBy(n => new {n.Category.Name, n.Exercise.Date})
                .Select(g => new ExpenseTotal
                {
                    Category = g.Key.Name,
                    Exercise = g.Key.Date,
                    Amount = g.Sum(e => e.Amount)
                });

            return result.ToList();
        }

        public IEnumerable<ExpenseTotal> GetGlobalTotalsByCategory()
        {
            var result = DbSet
                .Include(e => e.Category)
                .GroupBy(n => new {n.Category.Name})
                .Select(g => new ExpenseTotal
                {
                    Category = g.Key.Name,
                    Amount = g.Sum(e => e.Amount)
                });

            return result.ToList();
        }

        public IEnumerable<ExpenseTotal> GetGlobalTotalsByDate()
        {
            var result = DbSet
                .GroupBy(n => new {n.Date})
                .Select(g => new ExpenseTotal
                {
                    Date = g.Key.Date,
                    Amount = g.Sum(e => e.Amount)
                });

            return result.ToList();
        }
    }
}