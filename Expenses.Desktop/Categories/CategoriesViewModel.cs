using Expenses.Core;
using Expenses.Logic;
using Expenses.UI.Common;

namespace Expenses.UI.Categories
{
    public class CategoriesViewModel : EntitiesViewModel<Category>
    {
        public CategoriesViewModel() : base(CategoryService.Instance) {}
        public override string Title => "Catégories";
        protected override string EntityViewName => "CategoryView";
        protected override object GetEntityViewModel(Category entity) => CategoryViewModel.Instance(entity);
    }
}