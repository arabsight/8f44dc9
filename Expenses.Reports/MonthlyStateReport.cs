using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace Expenses.Reports
{
    public partial class MonthlyStateReport : XtraReport
    {
        private decimal _expense = 0;

        public MonthlyStateReport()
        {
            InitializeComponent();
        }

        private void SoldeLabel_SummaryGetResult(object sender, SummaryGetResultEventArgs e)
        {
            var withdrawals = GetCurrentColumnValue<decimal>("Withdrawals");
            e.Result = withdrawals - _expense;
            e.Handled = true;
        }

        private void SoldeLabel_SummaryRowChanged(object sender, EventArgs e)
        {
            _expense += GetCurrentColumnValue<decimal>("Amount");
        }
    }
}
