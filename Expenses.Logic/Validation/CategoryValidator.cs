using Expenses.Core;
using FluentValidation;

namespace Expenses.Logic.Validation
{
    public class CategoryValidator : AbstractValidator<Category>
    {
        private static CategoryValidator _instance;

        public CategoryValidator()
        {
            RuleFor(e => e.Name)
                .NotEmpty()
                .WithMessage("Nom du catégorie est obligatoire");
        }

        public static CategoryValidator Default => _instance ?? (_instance = new CategoryValidator());
    }
}