using System;
using System.Collections.ObjectModel;
using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using Expenses.Core.Shared;
using Expenses.Logic;

namespace Expenses.UI.Common
{
    [POCOViewModel]
    public abstract class EntitiesViewModel<TEntity> : ISupportNavigation, IViewModel 
        where TEntity : class, ITrackable
    {
        protected Service<TEntity> Service { get; }

        protected EntitiesViewModel(Service<TEntity> service)
        {
            Service = service;
            Messenger.Default.Register<EntityMessage<TEntity>>(this, OnMessage);
        }

        protected virtual void OnMessage(EntityMessage<TEntity> message)
        {
            switch (message.Type)
            {
                case MessageType.Added:
                    Entities.Add(Service.Find(message.Entity.Id));
                    break;
                case MessageType.Deleted:
                    throw new NotImplementedException();
                case MessageType.Modified:
                    Service.Reload(SelectedEntity);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public virtual Session Session => Session.Default;

        protected virtual IMessageBoxService MessageBoxService => null;
        protected virtual IDocumentManagerService DocumentManagerService => null;

        public virtual TEntity SelectedEntity { get; set; }
        public virtual ObservableCollection<TEntity> Entities { get; set; }
        public object Parameter { get; set; }

        [Command(false)]
        public virtual void OnNavigatedTo()
        {
            LoadEntities();
        }

        [Command(false)]
        public virtual void OnNavigatedFrom()
        {
        }

        public virtual string Title { get; set; }

        public virtual void Delete(TEntity entity)
        {
            var result = MessageBoxService.Show(
                "êtes-vous sûr de vouloir supprimer cet enregistrement?",
                "Attention",
                MessageButton.YesNo,
                MessageIcon.Warning,
                MessageResult.No
                );

            if (result != MessageResult.Yes) return;

            try
            {
                Service.Delete(entity);
                Entities.Remove(entity);
            }
            catch (Exception e)
            {
                MessageBoxService.Show(e.Message, "Erreur",
                    MessageButton.OK, MessageIcon.Error, MessageResult.OK);
            }
        }

        public virtual void New()
        {
            ShowDocument();
        }

        public virtual void Edit(TEntity entity)
        {
            ShowDocument(entity);
        }

        protected virtual void ShowDocument(TEntity entity = null)
        {
            var doc = DocumentManagerService
                .CreateDocument(EntityViewName, GetEntityViewModel(entity));
            doc.DestroyOnClose = true;
            doc.Show();
        }

        protected abstract string EntityViewName { get; }
        protected abstract object GetEntityViewModel(TEntity entity);

        protected virtual void LoadEntities()
        {
            Entities = Service.Get();
        }

        public virtual void Refresh()
        {
            LoadEntities();
        }

        public virtual bool CanNew()
        {
            return true;
        }

        public virtual bool CanEdit(TEntity entity)
        {
            return entity != null;
        }

        public virtual bool CanDelete(TEntity entity)
        {
            return entity != null;
        }
    }
}