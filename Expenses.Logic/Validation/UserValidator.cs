using Expenses.Core;
using FluentValidation;

namespace Expenses.Logic.Validation
{
    public class UserValidator : AbstractValidator<User>
    {
        private static UserValidator _instance;

        private UserValidator()
        {
            RuleFor(e => e.Fullname)
                .NotEmpty()
                .WithMessage("Nom et prénom obligatoire");

            RuleFor(e => e.Username)
                .NotEmpty()
                .WithMessage("Nom d'utilisateur obligatoire");

            RuleFor(e => e.Password)
                .NotEmpty()
                .WithMessage("Mot de passe obligatoire");
        }

        public static UserValidator Default => _instance ?? (_instance = new UserValidator());
    }
}