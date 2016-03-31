using Expenses.Core;
using Expenses.Logic.Validation;

namespace Expenses.Logic
{
    public class CategoryService : Service<Category>
    {
        public CategoryService() : base(CategoryValidator.Default) {}

        public static CategoryService Instance => new CategoryService();
    }
}