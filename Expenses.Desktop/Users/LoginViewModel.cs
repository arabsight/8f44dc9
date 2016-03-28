using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm.POCO;

namespace Expenses.UI.Users
{
    [POCOViewModel]
    public class LoginViewModel
    {
        public virtual string Username { get; set; }
        public virtual string Password { get; set; }
        public virtual string Message { get; set; }

        public void Reset()
        {
            Username = string.Empty;
            Password = string.Empty;
            Message = string.Empty;
        }

        protected LoginViewModel() {}

        public static LoginViewModel Instance => 
            ViewModelSource.Create(() => new LoginViewModel());
    }
}