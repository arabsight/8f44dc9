using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm.POCO;
using Expenses.Core;

namespace Expenses.UI.Exercises
{
    [POCOViewModel]
    public class ExerciseViewModel
    {
        protected ExerciseViewModel() {}

        public static ExerciseViewModel Instance => 
            ViewModelSource.Create(() => new ExerciseViewModel());

        public virtual string Message { get; set; }
        public virtual Exercise Exercise { get; set; }

        public void Reset()
        {
            Exercise = null;
            Message = string.Empty;
        }
    }
}