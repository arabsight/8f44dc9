using System;
using DevExpress.Mvvm.POCO;
using Expenses.Core;
using Expenses.Logic;
using Expenses.UI.Common;

// ReSharper disable VirtualMemberCallInContructor

namespace Expenses.UI.Withrdawals
{
    public class WithdrawalViewModel : EntityViewModel<Withdrawal>
    {
        protected WithdrawalViewModel(Withdrawal entity = null)
            : base(WithdrawalService.Instance)
        {
            SetEntity(entity);
        }

        public override object Title => "Alimentation de caisse";

        public static WithdrawalViewModel Instance(Withdrawal entity = null)
        {
            return ViewModelSource.Create(() => new WithdrawalViewModel(entity));
        }
        
        protected override void AddNew()
        {
            Entity = new Withdrawal
            {
                Date = DateTime.Today,
                Name = Session.Identity.Fullname,
                ExerciseId = Session.Exercise.Id,
                CreatedBy = Session.Identity.Id,
                UpdatedBy = Session.Identity.Id
            };

            Service.SetAdded(Entity);
        }
    }
}