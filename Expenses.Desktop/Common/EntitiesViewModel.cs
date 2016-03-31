using System.Collections.ObjectModel;
using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;

namespace Expenses.UI.Common
{
    [POCOViewModel]
    public abstract class EntitiesViewModel<TEntity> : ISupportNavigation, IViewModel where TEntity : class
    {
        public virtual Session Session => Session.Default;

        protected virtual IMessageBoxService MessageBoxService => null;
        protected virtual IDocumentManagerService DocumentManagerService => null;

        public virtual TEntity SelectedEntity { get; set; }
        public virtual ObservableCollection<TEntity> Entities { get; set; }
        public object Parameter { get; set; }

        [Command(false)]
        public virtual void OnNavigatedTo() {}

        [Command(false)]
        public virtual void OnNavigatedFrom() {}

        public virtual string Title { get; set; }
    }
}