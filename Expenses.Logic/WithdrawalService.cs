using System.Linq;
using Expenses.Core;
using Expenses.Logic.Validation;

namespace Expenses.Logic
{
    public class WithdrawalService : Service<Withdrawal>
    {
        public WithdrawalService() 
            : base(WithdrawalValidator.Default) {}

        public static WithdrawalService Instance => new WithdrawalService();

        public decimal GetTotalByExercise(Exercise exercise, bool withBalance = true)
        {
            var query = DbSet.Where(e => e.ExerciseId == exercise.Id);

            if (withBalance)
            {
                return query.Any() 
                    ? query.Sum(e => e.Amount) + exercise.Balance 
                    : exercise.Balance;
            }

            return query.Any() ? query.Sum(e => e.Amount) : 0;
        }
    }
}