using System;

namespace Expenses.Core.Shared
{
    public interface IUser
    {
        int Id { get; set; }
        string Fullname { get; set; }
        string Username { get; set; }
        string Password { get; set; }
        bool IsAdmin { get; set; }
        DateTime? LastLogin { get; set; }
    }
}