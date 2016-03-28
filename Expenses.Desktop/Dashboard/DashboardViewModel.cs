using DevExpress.Mvvm.POCO;
using Expenses.Core;
using Expenses.Logic;
using Expenses.UI.Common;

namespace Expenses.UI.Dashboard
{
    public class DashboardViewModel : EntitiesViewModel<Expense>
    {
        private readonly WithdrawalService _withdrawals;
        private readonly ExpenseService _expenses;

        public DashboardViewModel()
        {
            if (this.IsInDesignMode()) return;

            _expenses = ExpenseService.Instance;
            _withdrawals = WithdrawalService.Instance;
        }

        public override string Title => "Tableau de bord";

        public virtual decimal WithdrawalsTotal { get; set; }
        public virtual decimal ExpensesTotal { get; set; }
        public virtual decimal UntreatedExpensesTotal { get; set; }

        public virtual decimal Balance { get; set; }
        public virtual decimal RealBalance { get; set; }

        public override void OnNavigatedTo()
        {
            CalculateTotals();
        }

        private void CalculateTotals()
        {
            ExpensesTotal = _expenses.GetTotalByExercise(Session.Exercise);
            WithdrawalsTotal = _withdrawals.GetTotalByExercise(Session.Exercise);
            UntreatedExpensesTotal = 0;

            Balance = WithdrawalsTotal - ExpensesTotal;
            RealBalance = Balance - UntreatedExpensesTotal;
        }
    }
}