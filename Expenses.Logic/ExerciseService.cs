using Expenses.Core;
using Expenses.Logic.Validation;

namespace Expenses.Logic
{
    public class ExerciseService : Service<Exercise>
    {
        private ExerciseService() : base(ExerciseValidator.Default) {}

        public static ExerciseService Instance => new ExerciseService();

        public Exercise CurrentExercise => One(e => e.IsCurrent);

        public void Close(Exercise entity)
        {
            
        }
    }
}