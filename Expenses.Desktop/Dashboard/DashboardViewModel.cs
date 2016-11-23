using System.Collections.Generic;
using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using Expenses.Core.Helpers;
using Expenses.Logic;
using Expenses.UI.Common;

namespace Expenses.UI.Dashboard
{
    [POCOViewModel]
    public class DashboardViewModel : ISupportNavigation, IViewModel
    {
        private readonly ExpenseService _expenses;
        private readonly WithdrawalService _withdrawals;

        public DashboardViewModel()
        {
            _expenses = ExpenseService.Instance;
            _withdrawals = WithdrawalService.Instance;
        }

        public virtual Session Session => Session.Default;

        public virtual decimal MonthlyWithdrawalsTotal { get; set; }
        public virtual decimal MonthlyExpensesTotal { get; set; }
        public virtual decimal UntreatedExpensesTotal { get; set; }

        public virtual decimal Balance { get; set; }
        public virtual decimal RealBalance { get; set; }

        public virtual IEnumerable<ExpenseTotal> GlobalExpensesPerDate { get; set; }

        public virtual IEnumerable<ExpenseTotal> GlobalExpensesPerCategory { get; set; }
        public virtual IEnumerable<ExpenseTotal> MonthlyExpensesPerCategory { get; set; }

        public void OnNavigatedTo()
        {
            CalculateTotals();
            GetDataForCharts();
        }

        public void OnNavigatedFrom() {}

        public object Parameter { get; set; }

        public string Title => "Tableau de bord";

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