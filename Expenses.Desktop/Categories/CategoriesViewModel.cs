using System;
using DevExpress.Mvvm;
using Expenses.Core;
using Expenses.Logic;
using Expenses.UI.Common;

namespace Expenses.UI.Categories
{
    public class CategoriesViewModel : EntitiesViewModel<Category>
    {
        private readonly CategoryService _categories;

        public CategoriesViewModel()
        {
            _categories = CategoryService.Instance;
            Messenger.Default.Register<EntityMessage<Category>>(this, OnMessage);
        }

        public override string Title => "Catégories";

        public override void OnNavigatedTo()
        {
            LoadEntities();
        }

        public void LoadEntities()
        {
            Entities = _categories.Get();
        }

        public void New()
        {
            ShowDocument();
        }

        public void Edit(Category entity)
        {
            ShowDocument(entity);
        }

        private void ShowDocument(Category entity = null)
        {
            var vm = CategoryViewModel.Instance(entity);
            var document = DocumentManagerService.CreateDocument("CategoryView", vm);
            document.DestroyOnClose = true;
            document.Show();
        }

        public void Delete(Category entity)
        {
            var result = MessageBoxService.Show(
                "êtes-vous sûr de vouloir supprimer cette catégorie?",
                "Attention",
                MessageButton.YesNo,
                MessageIcon.Warning,
                MessageResult.No
                );

            if (result != MessageResult.Yes) return;

            try
            {
                _categories.Delete(entity);
                Entities.Remove(entity);
            }
            catch (Exception e)
            {
                MessageBoxService.Show(e.Message, "Erreur",
                    MessageButton.OK, MessageIcon.Error, MessageResult.OK);
            }
        }
        
        public bool CanEdit(Category entity)
        {
            return entity != null && Session.Identity.IsAdmin;
        }

        public bool CanDelete(Category entity)
        {
            return entity != null && Session.Identity.IsAdmin;
        }

        private void OnMessage(EntityMessage<Category> message)
        {
            switch (message.Type)
            {
                case MessageType.Added:
                    var added = _categories.Find(message.Entity.Id);
                    Entities.Add(added);
                    break;
                case MessageType.Modified:
                    _categories.Reload(SelectedEntity);
                    break;
                case MessageType.Deleted:
                    break;
            }
        }
    }
}