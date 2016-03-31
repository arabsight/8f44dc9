using Expenses.Core;
using FluentValidation;

namespace Expenses.Logic.Validation
{
    public class ReceiptTypeValidator : AbstractValidator<ReceiptType>
    {
        private static ReceiptTypeValidator _instance;

        public ReceiptTypeValidator()
        {
            RuleFor(e => e.Name)
                .NotEmpty()
                .WithMessage("Nom du catégorie est obligatoire");
        }

        public static ReceiptTypeValidator Default => _instance ?? (_instance = new ReceiptTypeValidator());
    }
}