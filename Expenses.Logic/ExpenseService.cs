using System.Collections.ObjectModel;
using System.Linq;
using Expenses.Core;
using Expenses.Logic.Validation;

namespace Expenses.Logic
{
    public class ExpenseService : Service<Expense>
    {
        public static ExpenseService Instance => new ExpenseService();

        public ObservableCollection<Expense> GetByExercise(int exercise)
        {
            return Get(e => e.ExerciseId == exercise, e => e.Category, e => e.ReceiptType);
        }

        public void TrySave(Expense entity)
        {
            Save(entity, ExpenseValidator.Default);
        }

        public decimal GetTotalByExercise(Exercise exercise)
        {
            return DbSet.Where(e => e.ExerciseId == exercise.Id).Sum(e => e.Amount);
        }
    }
}