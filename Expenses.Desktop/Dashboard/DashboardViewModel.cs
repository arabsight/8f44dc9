using System.Collections.Generic;
using Expenses.Core;
using Expenses.Core.Helpers;
using Expenses.Logic;
using Expenses.UI.Common;

namespace Expenses.UI.Dashboard
{
    public class DashboardViewModel : EntitiesViewModel<Expense>
    {
        private readonly ExpenseService _expenses;
        private readonly WithdrawalService _withdrawals;

        public DashboardViewModel()
        {
            _expenses = ExpenseService.Instance;
            _withdrawals = WithdrawalService.Instance;
        }

        public override string Title => "Tableau de bord";

        public virtual decimal MonthlyWithdrawalsTotal { get; set; }
        public virtual decimal MonthlyExpensesTotal { get; set; }
        public virtual decimal UntreatedExpensesTotal { get; set; }

        public virtual decimal Balance { get; set; }
        public virtual decimal RealBalance { get; set; }

        public virtual List<ExpenseTotal> GlobalExpensesPerDate { get; set; }

        public virtual List<ExpenseTotal> GlobalExpensesPerCategory { get; set; }
        public virtual List<ExpenseTotal> MonthlyExpensesPerCategory { get; set; }

        public override void OnNavigatedTo()
        {
            CalculateTotals();
            GetDataForCharts();
        }

        private void GetDataForCharts()
        {
            GlobalExpensesPerCategory = _expenses.GetGlobalTotalsByCategory();
            MonthlyExpensesPerCategory = _expenses.GetMonthlyTotalsByCategory(Session.Exercise);

            GlobalExpensesPerDate = _expenses.GetGlobalTotalsByDate();
        }

        private void CalculateTotals()
        {
            MonthlyExpensesTotal = _expenses.GetTotalByExercise(Session.Exercise);
            MonthlyWithdrawalsTotal = _withdrawals.GetTotalByExercise(Session.Exercise);
            UntreatedExpensesTotal = 0;

            Balance = MonthlyWithdrawalsTotal - MonthlyExpensesTotal;
            RealBalance = Balance - UntreatedExpensesTotal;
        }
    }
}