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
        
        public ExerciseSummaryViewModel(Exercise entity = null)
            : base(ExerciseService.Instance)
        {
            _withdrawals = WithdrawalService.Instance;
            _expenses = ExpenseService.Instance;

            SetEntity(entity);
            Entity.IsClosed = true;

            CalculateTotals();
        }

        public override object Title => "Exercise";

        public static ExerciseSummaryViewModel Instance(Exercise entity = null) =>
            ViewModelSource.Create(() => new ExerciseSummaryViewModel(entity));

        private void CalculateTotals()
        {
            ExpensesTotal = _expenses.GetTotalByExercise(Entity);
            WithdrawalsTotal = _withdrawals.GetTotalByExercise(Entity);
            Balance = WithdrawalsTotal + Entity.Balance - ExpensesTotal;
        }

        public virtual decimal Balance { get; set; }
        public virtual decimal WithdrawalsTotal { get; set; }
        public virtual decimal ExpensesTotal { get; set; }
    }
}