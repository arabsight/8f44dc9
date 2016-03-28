using Expenses.Core;
using FluentValidation;

namespace Expenses.Logic.Validation
{
    public class WithdrawalValidator : AbstractValidator<Withdrawal>
    {
        private static WithdrawalValidator _instance;

        private WithdrawalValidator()
        {
            RuleFor(e => e.ExerciseId)
                .NotEmpty()
                .WithMessage("Aucun Exercise défini");

            RuleFor(e => e.Name)
                .NotEmpty()
                .WithMessage("Nom du récipiendaire obligatoire");

            RuleFor(e => e.Date)
                .NotNull()
                .WithMessage("Date de réception est obligatoire");

            RuleFor(e => e.Amount)
                .NotEmpty()
                .WithMessage("Montant reçu est obligatoire");
        }

        public static WithdrawalValidator Default => _instance ?? (_instance = new WithdrawalValidator());
    }
}