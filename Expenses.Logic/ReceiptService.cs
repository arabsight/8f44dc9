using Expenses.Core;

namespace Expenses.Logic
{
    public class ReceiptService : Service<ReceiptType>
    {
        public static ReceiptService Instance => new ReceiptService();
    }
}