using DevExpress.Mvvm.POCO;
using Expenses.Core;
using Expenses.Logic;
using Expenses.UI.Common;

// ReSharper disable VirtualMemberCallInContructor

namespace Expenses.UI.Receipts
{
    public class ReceiptViewModel : EntityViewModel<ReceiptType>
    {
        public ReceiptViewModel(ReceiptType entity = null) :
            base(ReceiptService.Instance)
        {
            SetEntity(entity);
        }

        public override object Title => "Document";

        public static ReceiptViewModel Instance(ReceiptType entity = null) =>
            ViewModelSource.Create(() => new ReceiptViewModel(entity));

        //    if (entity == null) AddNew();
        //{

        //private void SetEntity(ReceiptType entity = null)
        //    else Entity = Service.Find(entity.Id);
        //}

        //protected override void AddNew()
        //{
        //    Entity = new ReceiptType
        //    {
        //        CreatedBy = Session.Identity.Id,
        //        UpdatedBy = Session.Identity.Id
        //    };

        //    Service.SetAdded(Entity);
        //}
    }
}