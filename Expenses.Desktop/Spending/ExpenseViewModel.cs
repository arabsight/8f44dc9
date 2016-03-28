using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm;
using DevExpress.Mvvm.POCO;
using Expenses.Core;
using Expenses.Logic;
using Expenses.UI.Common;

// ReSharper disable VirtualMemberCallInContructor

namespace Expenses.UI.Spending
{
    [POCOViewModel]
    public class ExpenseViewModel : IDocumentContent
    {
        private ExpenseService _expenses;
        private CategoryService _categories;
        private ReceiptService _receipts;

        protected ExpenseViewModel()
        {
            if (!this.IsInDesignMode())
                throw new Exception("Design Mode Only");
            //InitServices();
        }

        private void InitServices()
        {
            _expenses = ExpenseService.Instance;
            _categories = CategoryService.Instance;
            _receipts = ReceiptService.Instance;
        }

        protected ExpenseViewModel(Session session, Expense entity = null)
        {
            InitServices();
            Receipts = _receipts.Get();
            Categories = _categories.Get();
            Session = session;
            SetEntity(entity);
        }

        public static ExpenseViewModel Instance(Session session, Expense entity = null)
        {
            return ViewModelSource.Create(() => new ExpenseViewModel(session, entity));
        }

        public Session Session { get; }
        public virtual Expense Entity { get; set; }

        public virtual ObservableCollection<Category> Categories { get; set; }
        public virtual ObservableCollection<ReceiptType> Receipts { get; set; }

        public virtual IMessageBoxService MessageBoxService => null;

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

                _expenses.SetAdded(Entity);
            }
            else
            {
                Entity = _expenses.Find(entity.Id);
            }
        }

        // Commands
        public void SaveAndClose()
        {
            if (TrySave()) DocumentOwner.Close(this, false);
        }

        public void SaveAndNew()
        {
            if (TrySave()) SetEntity(null);
        }

        private bool TrySave()
        {
            if (!_expenses.IsDirty(Entity)) return true;

            try
            {
                if (_expenses.State(Entity) == EntityState.Added)
                {
                    _expenses.TrySave(Entity);
                    Messenger.Default.Send(new EntityMessage<Expense>(Entity, MessageType.Added));
                }
                else
                {
                    Entity.UpdatedBy = Session.Identity.Id;
                    _expenses.TrySave(Entity);
                    Messenger.Default.Send(new EntityMessage<Expense>(Entity, MessageType.Modified));
                }

                return true;
            }
            catch (Exception e)
            {
                MessageBoxService.Show(e.Message, "Validation");
                return false;
            }
        }

        public void AddCategory()
        {
            
        }

        public void AddReceipt()
        {
            
        }

        public void OnClose(CancelEventArgs e)
        {
            if (!_expenses.IsDirty(Entity)) return;

            var result = ShowConfirmation();

            switch (result)
            {
                case MessageResult.Yes:
                    if (!TrySave()) e.Cancel = true;
                    break;
                case MessageResult.Cancel:
                    e.Cancel = true;
                    break;
                case MessageResult.None:
                    break;
                case MessageResult.OK:
                    break;
                case MessageResult.No:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void OnDestroy() {}
        public IDocumentOwner DocumentOwner { get; set; }
        public object Title => "Dépenses";

        private MessageResult ShowConfirmation()
        {
            return MessageBoxService.Show(
                "Voulez vous enregistrer les modifications", 
                "Enregistrer", 
                MessageButton.YesNoCancel, 
                MessageIcon.Question, 
                MessageResult.No);
        }
    }
}