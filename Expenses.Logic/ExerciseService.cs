using System;
using Expenses.Core;
using Expenses.Logic.Validation;

namespace Expenses.Logic
{
    public class ExerciseService : Service<Exercise>
    {
        private ExerciseService() : base(ExerciseValidator.Default) {}

        public static ExerciseService Instance => new ExerciseService();

        public Exercise Create(Exercise exercise)
        {
            exercise.Date = new DateTime(exercise.Date.Year, exercise.Date.Month, 1);
            SetAdded(exercise);
            Save(exercise);
            return exercise;
        }
    }
}