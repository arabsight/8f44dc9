using DevExpress.Mvvm.POCO;
using Expenses.Core;
using Expenses.Logic;
using Expenses.UI.Common;

// ReSharper disable VirtualMemberCallInContructor

namespace Expenses.UI.Users
{
    public class UserViewModel : EntityViewModel<User>
    {
        public UserViewModel(User entity = null)
            : base(UserService.Instance)
        {
            SetEntity(entity);
        }

        public override object Title => "Utilisateur";

        public static UserViewModel Instance(User entity = null) =>
            ViewModelSource.Create(() => new UserViewModel(entity));
    }
}