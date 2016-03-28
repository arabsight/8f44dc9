using Expenses.Core;
using FluentValidation;

namespace Expenses.Logic.Validation
{
    public class ExpenseValidator : AbstractValidator<Expense>
    {
        private static ExpenseValidator _instance;

        private ExpenseValidator()
        {
            RuleFor(e => e.ExerciseId)
                .NotEmpty()
                .WithMessage("Aucun Exercise défini");

            RuleFor(e => e.CategoryId)
                .NotEmpty()
                .WithMessage("Catégorie est obligatoire");

            RuleFor(e => e.Name)
                .NotEmpty()
                .WithMessage("Nom & Prénom obligatoires");

            RuleFor(e => e.Date)
                .NotNull()
                .WithMessage("Date de dépense est obligatoire");

            RuleFor(e => e.Amount)
                .NotEmpty()
                .WithMessage("Montant dépensée est obligatoire");

            RuleFor(e => e.ReceiptTypeId)
                .NotEmpty()
                .WithMessage("Type Document obligatoire");

            RuleFor(e => e.ReceiptNumber)
                .NotEmpty()
                .WithMessage("N° Bon obligatoire");
        }

        public static ExpenseValidator Default => _instance ?? (_instance = new ExpenseValidator());
    }
}