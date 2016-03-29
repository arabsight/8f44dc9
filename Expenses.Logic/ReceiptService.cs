using Expenses.Core;
using Expenses.Logic.Validation;

namespace Expenses.Logic
{
    public class ReceiptService : Service<ReceiptType>
    {
        public ReceiptService() : base(ReceiptTypeValidator.Default) {}

        public static ReceiptService Instance => new ReceiptService();
    }
}