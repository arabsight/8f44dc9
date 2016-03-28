using System;
using DevExpress.Mvvm;
using Expenses.Core.Shared;

namespace Expenses.Core
{
    public class Withdrawal : BindableBase, ITrackable
    {
        public int Id { get; set; }

        public int ExerciseId
        {
            get { return GetProperty(() => ExerciseId); }
            set { SetProperty(() => ExerciseId, value); }
        }

        public DateTime Date
        {
            get { return GetProperty(() => Date); }
            set { SetProperty(() => Date, value); }
        }

        public string Name
        {
            get { return GetProperty(() => Name); }
            set { SetProperty(() => Name, value); }
        }

        public decimal Amount
        {
            get { return GetProperty(() => Amount); }
            set { SetProperty(() => Amount, value); }
        }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }


        public Exercise Exercise { get; set; }
    }
}