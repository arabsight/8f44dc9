using System;
using System.ComponentModel;
using System.Data.Entity;
using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using Expenses.Core.Shared;
using Expenses.Logic;

namespace Expenses.UI.Common
{
    [POCOViewModel]
    public class EntityViewModel<TEntity> : IDocumentContent where TEntity : class, new()
    {
        protected Service<TEntity> Service;

        public EntityViewModel(Service<TEntity> service)
        {
            Service = service;
        }

        public virtual Session Session => Session.Default;

        public virtual TEntity Entity { get; set; }

        protected virtual IMessageBoxService MessageBoxService => null;

        [Command(false)]
        public virtual void OnClose(CancelEventArgs e)
        {
            if (!Service.IsDirty(Entity)) return;
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

        [Command(false)]
        public virtual void OnDestroy()
        {
        }

        public virtual IDocumentOwner DocumentOwner { get; set; }
        public virtual object Title { get; } = string.Empty;

        protected virtual MessageResult ShowConfirmation()
        {
            return MessageBoxService.Show(
                "Voulez vous enregistrer les modifications",
                "Enregistrer",
                MessageButton.YesNoCancel,
                MessageIcon.Question,
                MessageResult.No);
        }

        protected virtual bool TrySave()
        {
            if (!Service.IsDirty(Entity)) return true;

            try
            {
                if (Service.State(Entity) == EntityState.Added)
                {
                    Service.Save(Entity);
                    Messenger.Default.Send(new EntityMessage<TEntity>(Entity, MessageType.Added));
                }
                else
                {
                    ((ITrackable) Entity).UpdatedBy = Session.Identity.Id;
                    Service.Save(Entity);
                    Messenger.Default.Send(new EntityMessage<TEntity>(Entity, MessageType.Modified));
                }

                return true;
            }
            catch (Exception e)
            {
                MessageBoxService.Show(e.Message, "Validation");
                return false;
            }
        }

        public virtual void SaveAndClose()
        {
            if (TrySave()) DocumentOwner.Close(this, false);
        }

        public virtual void SaveAndNew()
        {
            if (TrySave()) AddNew();
        }

        protected virtual void AddNew()
        {
            Entity = new TEntity();

            ((ITrackable) Entity).CreatedBy = Session.Identity.Id;
            ((ITrackable) Entity).UpdatedBy = Session.Identity.Id;

            Service.SetAdded(Entity);
        }

        protected virtual void SetEntity(TEntity entity = null)
        {
            if (entity == null) AddNew();
            else Entity = Service.Find(((ITrackable)entity).Id);
        }
    }
}