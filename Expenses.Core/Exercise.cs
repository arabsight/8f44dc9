using System;
using System.Collections.ObjectModel;
using DevExpress.Mvvm;
using Expenses.Core.Shared;

// ReSharper disable VirtualMemberCallInContructor

namespace Expenses.Core
{
    public class Exercise : BindableBase, ITrackable
    {
        public Exercise()
        {
            IsCurrent = true;
            IsClosed = false;
            Expenses = new ObservableCollection<Expense>();
            Withdrawals = new ObservableCollection<Withdrawal>();
        }

        public int Id { get; set; }

        public DateTime Date
        {
            get { return GetProperty(() => Date); }
            set { SetProperty(() => Date, value, OnDateChanged); }
        }

        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }

        public bool IsCurrent
        {
            get { return GetProperty(() => IsCurrent); }
            set { SetProperty(() => IsCurrent, value); }
        }

        public bool IsClosed
        {
            get { return GetProperty(() => IsClosed); }
            set { SetProperty(() => IsClosed, value); }
        }

        public decimal Balance
        {
            get { return GetProperty(() => Balance); }
            set { SetProperty(() => Balance, value); }
        }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }

        private void OnDateChanged()
        {
            StartDate = new DateTime(Date.Year, Date.Month, 1);
            EndDate = new DateTime(Date.Year, Date.Month, DateTime.DaysInMonth(Date.Year, Date.Month));
        }

        public DateTime GetLastDay()
        {
            return DateTime.Today < EndDate ? DateTime.Today : EndDate;
        }

        public virtual ObservableCollection<Expense> Expenses { get; set; }
        public virtual ObservableCollection<Withdrawal> Withdrawals { get; set; }
    }
}