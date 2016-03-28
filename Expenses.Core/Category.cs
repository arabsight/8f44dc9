using System;
using System.Collections.ObjectModel;
using DevExpress.Mvvm;
using Expenses.Core.Shared;

// ReSharper disable VirtualMemberCallInContructor

namespace Expenses.Core
{
    public class Category : BindableBase, ITrackable
    {
        public Category()
        {
            Expenses = new ObservableCollection<Expense>();
        }

        public int Id
        {
            get { return GetProperty(() => Id); }
            set { SetProperty(() => Id, value); }
        }

        public string Name
        {
            get { return GetProperty(() => Name); }
            set { SetProperty(() => Name, value); }
        }

        public virtual ObservableCollection<Expense> Expenses { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
    }
}