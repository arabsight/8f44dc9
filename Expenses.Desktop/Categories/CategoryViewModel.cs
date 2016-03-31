using DevExpress.Mvvm.POCO;
using Expenses.Core;
using Expenses.Logic;
using Expenses.UI.Common;

// ReSharper disable VirtualMemberCallInContructor

namespace Expenses.UI.Categories
{
    public class CategoryViewModel : EntityViewModel<Category>
    {
        public CategoryViewModel(Category entity)
            : base(CategoryService.Instance)
        {
            SetEntity(entity);
        }

        public override object Title => "Catégorie";

        public static CategoryViewModel Instance(Category entity = null) =>
            ViewModelSource.Create(() => new CategoryViewModel(entity));
    }
}