using Expenses.Core;
using Expenses.Logic.Validation;

namespace Expenses.Logic
{
    public class ExerciseService : Service<Exercise>
    {
        public static ExerciseService Instance => new ExerciseService();

        public Exercise CurrentExercise => One(e => e.IsCurrent);

        public void TrySave(Exercise entity)
        {
            Save(entity, ExerciseValidator.Default);
        }
    }
}