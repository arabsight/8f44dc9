using Expenses.Core;
using FluentValidation;

namespace Expenses.Logic.Validation
{
    public class ExerciseValidator : AbstractValidator<Exercise>
    {
        private static ExerciseValidator _instance;

        public ExerciseValidator()
        {
            RuleFor(e => e.Date)
                .NotNull()
                .WithMessage("Mois d'exercise est obligatoire");
        }

        public static ExerciseValidator Default => _instance ?? (_instance = new ExerciseValidator());
    }
}