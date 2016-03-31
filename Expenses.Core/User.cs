using System;
using DevExpress.Mvvm;
using Expenses.Core.Shared;

namespace Expenses.Core
{
    public class User : BindableBase, ITrackable, IUser
    {
        public User()
        {
            IsAdmin = false;
        }

        public int Id { get; set; }

        public string Fullname
        {
            get { return GetProperty(() => Fullname); }
            set { SetProperty(() => Fullname, value); }
        }

        public string Username
        {
            get { return GetProperty(() => Username); }
            set { SetProperty(() => Username, value); }
        }

        public string Password
        {
            get { return GetProperty(() => Password); }
            set { SetProperty(() => Password, value); }
        }

        public bool IsAdmin
        {
            get { return GetProperty(() => IsAdmin); }
            set { SetProperty(() => IsAdmin, value); }
        }

        public DateTime? LastLogin { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
    }
}