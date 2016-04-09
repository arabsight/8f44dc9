using System;

namespace Expenses.Core.Helpers
{
    public class ExpenseTotal
    {
        public string Category { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public DateTime Exercise { get; set; }
        public decimal Withdrawals { get; set; }
    }
}