using DevExpress.Mvvm.POCO;
using Expenses.Core;
using Expenses.Logic;
using Expenses.UI.Common;

// ReSharper disable VirtualMemberCallInContructor

namespace Expenses.UI.Exercises
{
    public class ExerciseSummaryViewModel : EntityViewModel<Exercise>
    {
        private readonly ExpenseService _expenses;
        private readonly WithdrawalService _withdrawals;
        
        public ExerciseSummaryViewModel(Exercise oldExercise = null)
            : base(ExerciseService.Instance)
        {
            _withdrawals = WithdrawalService.Instance;
            _expenses = ExpenseService.Instance;

            //SetEntity(oldExercise);
            //Entity.IsClosed = true;

            CalculateTotals(oldExercise);
        }

        public override object Title => "Exercise";

        public static ExerciseSummaryViewModel Instance(Exercise entity = null) =>
            ViewModelSource.Create(() => new ExerciseSummaryViewModel(entity));

        private void CalculateTotals(Exercise oldExercise)
        {
            ExpensesTotal = _expenses.GetTotalByExercise(oldExercise);
            WithdrawalsTotal = _withdrawals.GetTotalByExercise(oldExercise);
            Balance = WithdrawalsTotal + Entity.Balance - ExpensesTotal;
        }

        public virtual decimal Balance { get; set; }
        public virtual decimal WithdrawalsTotal { get; set; }
        public virtual decimal ExpensesTotal { get; set; }
    }
}