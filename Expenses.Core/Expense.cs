using System;
using DevExpress.Mvvm;
using Expenses.Core.Shared;

namespace Expenses.Core
{
    public class Expense : BindableBase, ITrackable
    {
        public Expense()
        {
            IsValidated = true;
        }

        public int Id { get; set; }

        public int ExerciseId
        {
            get { return GetProperty(() => ExerciseId); }
            set { SetProperty(() => ExerciseId, value); }
        }

        public int CategoryId
        {
            get { return GetProperty(() => CategoryId); }
            set { SetProperty(() => CategoryId, value); }
        }

        public string Name
        {
            get { return GetProperty(() => Name); }
            set { SetProperty(() => Name, value); }
        }

        public DateTime Date
        {
            get { return GetProperty(() => Date); }
            set { SetProperty(() => Date, value); }
        }

        public decimal Amount
        {
            get { return GetProperty(() => Amount); }
            set { SetProperty(() => Amount, value); }
        }

        public bool IsValidated
        {
            get { return GetProperty(() => IsValidated); }
            set { SetProperty(() => IsValidated, value); }
        }

        public int ReceiptTypeId
        {
            get { return GetProperty(() => ReceiptTypeId); }
            set { SetProperty(() => ReceiptTypeId, value); }
        }

        public string ReceiptNumber
        {
            get { return GetProperty(() => ReceiptNumber); }
            set { SetProperty(() => ReceiptNumber, value); }
        }

        public string Description
        {
            get { return GetProperty(() => Description); }
            set { SetProperty(() => Description, value); }
        }

        public Exercise Exercise { get; set; }
        public Category Category { get; set; }
        public ReceiptType ReceiptType { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
    }
}