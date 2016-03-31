using System;
using DevExpress.Mvvm;
using Expenses.Core;
using Expenses.Logic;
using Expenses.UI.Common;

namespace Expenses.UI.Receipts
{
    public class ReceiptsViewModel : EntitiesViewModel<ReceiptType>
    {
        private readonly ReceiptService _receipts;

        public ReceiptsViewModel()
        {
            _receipts = ReceiptService.Instance;
            Messenger.Default.Register<EntityMessage<ReceiptType>>(this, OnMessage);
        }

        public override string Title => "Documents";

        private void OnMessage(EntityMessage<ReceiptType> message)
        {
            switch (message.Type)
            {
                case MessageType.Added:
                    var added = _receipts.Find(message.Entity.Id);
                    Entities.Add(added);
                    break;
                case MessageType.Modified:
                    _receipts.Reload(SelectedEntity);
                    break;
                case MessageType.Deleted:
                    break;
            }
        }

        public override void OnNavigatedTo()
        {
            LoadEntities();
        }

        public void LoadEntities()
        {
            Entities = _receipts.Get();
        }

        public void New()
        {
            ShowDocument();
        }

        public void Edit(ReceiptType entity)
        {
            ShowDocument(entity);
        }

        private void ShowDocument(ReceiptType entity = null)
        {
            var vm = ReceiptViewModel.Instance(entity);
            var document = DocumentManagerService.CreateDocument("ReceiptView", vm);
            document.DestroyOnClose = true;
            document.Show();
        }

        public void Delete(ReceiptType entity)
        {
            var result = MessageBoxService.Show(
                "êtes-vous sûr de vouloir supprimer ce document?",
                "Attention",
                MessageButton.YesNo,
                MessageIcon.Warning,
                MessageResult.No
                );

            if (result != MessageResult.Yes) return;

            try
            {
                _receipts.Delete(entity);
                Entities.Remove(entity);
            }
            catch (Exception e)
            {
                MessageBoxService.Show(e.Message, "Erreur",
                    MessageButton.OK, MessageIcon.Error, MessageResult.OK);
            }
        }

        public bool CanEdit(ReceiptType entity)
        {
            return entity != null && Session.Identity.IsAdmin;
        }

        public bool CanDelete(ReceiptType entity)
        {
            return entity != null && Session.Identity.IsAdmin;
        }
    }
}