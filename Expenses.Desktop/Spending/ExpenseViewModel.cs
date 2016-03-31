using System;
using System.Collections.ObjectModel;
using DevExpress.Mvvm;
using DevExpress.Mvvm.POCO;
using Expenses.Core;
using Expenses.Logic;
using Expenses.UI.Categories;
using Expenses.UI.Common;
using Expenses.UI.Receipts;

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

            Messenger.Default.Register<EntityMessage<Category>>(this, OnCategoryMessage);
            Messenger.Default.Register<EntityMessage<ReceiptType>>(this, OnReceiptMessage);
        }

        private void OnCategoryMessage(EntityMessage<Category> msg)
        {
            if(msg.Type != MessageType.Added) return;
            var category = _categories.Find(msg.Entity.Id);
            Categories.Add(category);
            Entity.CategoryId = category.Id;
        }

        private void OnReceiptMessage(EntityMessage<ReceiptType> msg)
        {
            if (msg.Type != MessageType.Added) return;
            var receipt = _receipts.Find(msg.Entity.Id);
            Receipts.Add(receipt);
            Entity.ReceiptTypeId = receipt.Id;
        }


        public virtual ObservableCollection<Category> Categories { get; set; }
        public virtual ObservableCollection<ReceiptType> Receipts { get; set; }

        public override object Title => "Dépenses";
        protected virtual IDocumentManagerService DocumentManagerService => null;

        public static ExpenseViewModel Instance(Expense entity = null)
        {
            return ViewModelSource.Create(() => new ExpenseViewModel(entity));
        }
        
        protected override void AddNew()
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
        
        public void AddCategory()
        {
            ShowDocument(CategoryViewModel.Instance(), "CategoryView");
        }

        public void AddReceipt()
        {
            ShowDocument(ReceiptViewModel.Instance(), "ReceiptView");
        }

        private void ShowDocument(object vm, string view)
        {
            var document = DocumentManagerService.CreateDocument(view, vm);
            document.DestroyOnClose = true;
            document.Show();
        }
    }
}