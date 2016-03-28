using System;
using System.ComponentModel;
using System.Data.Entity;
using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm.POCO;
using Expenses.Core;
using Expenses.Logic;
using Expenses.UI.Common;

namespace Expenses.UI.Withrdawals
{
    [POCOViewModel]
    public class WithdrawalViewModel : IDocumentContent
    {
        private readonly WithdrawalService _withdrawals;

        protected WithdrawalViewModel()
        {
            if (!this.IsInDesignMode())
                throw new Exception("Design Mode Only");
        }

        protected WithdrawalViewModel(Session session, Withdrawal entity = null)
        {
            _withdrawals = WithdrawalService.Instance;
            Session = session;
            SetEntity(entity);
        }

        public Session Session { get; }
        public virtual Withdrawal Entity { get; set; }

        protected virtual IMessageBoxService MessageBoxService => null;

        public void OnClose(CancelEventArgs e)
        {
            if (!_withdrawals.IsDirty(Entity)) return;

            var result = ShowConfirmation();

            if (result == MessageResult.Yes)
            {
                if (!TrySave()) e.Cancel = true;
            }
            else if (result == MessageResult.Cancel)
            {
                e.Cancel = true;
            }
        }

        public void OnDestroy() {}

        public IDocumentOwner DocumentOwner { get; set; }
        public object Title => "Retrait";

        public static WithdrawalViewModel Instance(Session session, Withdrawal entity = null)
        {
            return ViewModelSource.Create(() => new WithdrawalViewModel(session, entity));
        }

        private void SetEntity(Withdrawal entity)
        {
            if (entity == null)
            {
                Entity = new Withdrawal
                {
                    Date = DateTime.Today,
                    Name = Session.Identity.Fullname,
                    ExerciseId = Session.Exercise.Id,
                    CreatedBy = Session.Identity.Id,
                    UpdatedBy = Session.Identity.Id
                };

                _withdrawals.SetAdded(Entity);
            }
            else
            {
                Entity = _withdrawals.Find(entity.Id);
            }
        }

        private bool TrySave()
        {
            if (!_withdrawals.IsDirty(Entity)) return true;

            try
            {
                if (_withdrawals.State(Entity) == EntityState.Added)
                {
                    _withdrawals.TrySave(Entity);
                    Messenger.Default.Send(new EntityMessage<Withdrawal>(Entity, MessageType.Added));
                }
                else
                {
                    Entity.UpdatedBy = Session.Identity.Id;
                    _withdrawals.TrySave(Entity);
                    Messenger.Default.Send(new EntityMessage<Withdrawal>(Entity, MessageType.Modified));
                }

                return true;
            }
            catch (Exception e)
            {
                MessageBoxService.Show(e.Message, "Validation");
                return false;
            }
        }

        public void SaveAndClose()
        {
            if (TrySave()) DocumentOwner.Close(this, false);
        }

        public void SaveAndNew()
        {
            if (TrySave()) SetEntity(null);
        }

        private MessageResult ShowConfirmation()
        {
            return MessageBoxService.Show("Voulez vous enregistrer les modifications", "Enregistrer",
                MessageButton.YesNoCancel, MessageIcon.Question, MessageResult.No);
        }
    }
}