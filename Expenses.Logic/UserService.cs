using System;
using Expenses.Core;
using Expenses.Core.Shared;
using Expenses.Logic.Validation;

namespace Expenses.Logic
{
    public class UserService : Service<User>
    {
        public UserService() : base(UserValidator.Default) {}

        public static UserService Instance => new UserService();

        public IUser Identity { get; private set; }

        public bool Attempt(string username, string password)
        {
            var user = One(e => e.Username == username && e.Password == password);
            if (user == null) return false;
            UpdateLastLogin(user);
            Identity = user;
            return true;
        }

        private void UpdateLastLogin(User user)
        {
            user.LastLogin = DateTime.Now;
            user.UpdatedBy = user.Id;
            Save();
        }
    }
}