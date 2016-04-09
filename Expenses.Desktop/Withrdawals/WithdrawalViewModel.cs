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
            base.AddNew();
            Entity.Name = Session.Identity.Fullname;
            Entity.ExerciseId = Session.Exercise.Id;
            Entity.Date = Session.Exercise.GetLastDay();
        }
    }
}