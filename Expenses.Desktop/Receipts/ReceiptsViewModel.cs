using Expenses.Core;
using Expenses.Logic;
using Expenses.UI.Common;

namespace Expenses.UI.Receipts
{
    public class ReceiptsViewModel : EntitiesViewModel<ReceiptType>
    {
        public ReceiptsViewModel() : base(ReceiptService.Instance) {}
        public override string Title => "Documents";
        protected override string EntityViewName => "ReceiptView";
        protected override object GetEntityViewModel(ReceiptType entity) => ReceiptViewModel.Instance(entity);
    }
}