using Expenses.Core;
using Expenses.Logic;
using Expenses.UI.Common;

namespace Expenses.UI.Users
{
    public class UsersViewModel : EntitiesViewModel<User>
    {
        //private readonly UserService _users;

        public UsersViewModel() : base(UserService.Instance)
        {
            //_users = UserService.Instance;
            //Messenger.Default.Register<EntityMessage<User>>(this, OnMessage);
        }

        public override string Title => "Utilisateurs";

        //public void LoadEntities()
        //{
        //    Entities = _users.Get();
        //}

        //public void New()
        //{
        //    ShowDocument();
        //}

        //public void Edit(User entity)
        //{
        //    ShowDocument(entity);
        //}

        //private void ShowDocument(User entity = null)
        //{
        //    var vm = UserViewModel.Instance(entity);
        //    var document = DocumentManagerService.CreateDocument("UserView", vm);
        //    document.DestroyOnClose = true;
        //    document.Show();
        //}

        protected override string EntityViewName => "UserView";

        //private void OnMessage(EntityMessage<User> message)
        //{
        //    switch (message.Type)
        //    {
        //        case MessageType.Added:
        //            var added = _users.Find(message.Entity.Id);
        //            Entities.Add(added);
        //            break;
        //        case MessageType.Modified:
        //            _users.Reload(SelectedEntity);
        //            break;
        //        case MessageType.Deleted:
        //            break;
        //    }
        //}

        //public override void OnNavigatedTo()
        //{
        //    LoadEntities();
        //}

        protected override object GetEntityViewModel(User entity) => UserViewModel.Instance(entity);

        //public void Delete(User entity)
        //{
        //    var result = MessageBoxService.Show(
        //        "êtes-vous sûr de vouloir supprimer cet utilisateur?",
        //        "Attention",
        //        MessageButton.YesNo,
        //        MessageIcon.Warning,
        //        MessageResult.No
        //        );

        //    if (result != MessageResult.Yes) return;

        //    try
        //    {
        //        _users.Delete(entity);
        //        Entities.Remove(entity);
        //    }
        //    catch (Exception e)
        //    {
        //        MessageBoxService.Show(e.Message, "Erreur",
        //            MessageButton.OK, MessageIcon.Error, MessageResult.OK);
        //    }
        //}

        public override bool CanNew() => Session.Identity.IsAdmin;

        public override bool CanEdit(User entity)
        {
            return base.CanEdit(entity) &&
                   (Session.Identity.IsAdmin || entity.Id == Session.Identity.Id);
        }

        public override bool CanDelete(User entity)
        {
            return base.CanDelete(entity) && Session.Identity.IsAdmin;
        }
    }
}