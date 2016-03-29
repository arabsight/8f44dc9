using System;
using System.Collections.ObjectModel;
using DevExpress.Mvvm.POCO;
using Expenses.Core;
using Expenses.Logic;
using Expenses.UI.Common;

// ReSharper disable VirtualMemberCallInContructor

namespace Expenses.UI.Spending
{
    public class ExpenseViewModel : EntityViewModel<Expense>
    {
        private readonly CategoryService _categories;
        private readonly ReceiptService _receipts;

        protected ExpenseViewModel(Expense entity = null)
            : base(ExpenseService.Instance)
        {
            _categories = CategoryService.Instance;
            _receipts = ReceiptService.Instance;

            Receipts = _receipts.Get();
            Categories = _categories.Get();

            SetEntity(entity);
        }

        public virtual ObservableCollection<Category> Categories { get; set; }
        public virtual ObservableCollection<ReceiptType> Receipts { get; set; }

        public override object Title => "Dépenses";
        
        public static ExpenseViewModel Instance(Expense entity = null)
        {
            return ViewModelSource.Create(() => new ExpenseViewModel(entity));
        }

        private void SetEntity(Expense entity)
        {
            if (entity == null)
            {
                Entity = new Expense
                {
                    Date = DateTime.Today,
                    ExerciseId = Session.Exercise.Id,
                    CreatedBy = Session.Identity.Id,
                    UpdatedBy = Session.Identity.Id
                };

                Service.SetAdded(Entity);
            }
            else
            {
                Entity = Service.Find(entity.Id);
            }
        }
        
        public void SaveAndNew()
        {
            if (TrySave()) SetEntity(null);
        }
        
        public void AddCategory()
        {
        }

        public void AddReceipt()
        {
        }
    }
}