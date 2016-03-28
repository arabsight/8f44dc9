using Expenses.Core;

namespace Expenses.Logic
{
    public class CategoryService : Service<Category>
    {
        public static CategoryService Instance => new CategoryService();
    }
}