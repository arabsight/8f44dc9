using System.Collections.ObjectModel;
using System.Linq;
using Expenses.Core;
using Expenses.Logic.Validation;

namespace Expenses.Logic
{
    public class WithdrawalService : Service<Withdrawal>
    {
        public static WithdrawalService Instance => new WithdrawalService();

        public ObservableCollection<Withdrawal> GetByExercise(int exercise)
        {
            return Get(e => e.ExerciseId == exercise);
        }

        public void TrySave(Withdrawal entity)
        {
            Save(entity, WithdrawalValidator.Default);
        }

        public decimal GetTotalByExercise(Exercise exercise)
        {
            return DbSet.Where(e => e.ExerciseId == exercise.Id).Sum(e => e.Amount) + exercise.Balance;
        }
    }
}