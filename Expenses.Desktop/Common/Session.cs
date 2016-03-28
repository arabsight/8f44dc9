using DevExpress.Mvvm;
using Expenses.Core;
using Expenses.Core.Shared;

namespace Expenses.UI.Common
{
    public class Session : BindableBase
    {
        private static Session _session;

        private Session() {}

        public static Session Default => _session ?? (_session = new Session());

        public IUser Identity
        {
            get { return GetProperty(() => Identity); }
            set { SetProperty(() => Identity, value); }
        }

        public Exercise Exercise {
            get { return GetProperty(() => Exercise); }
            set { SetProperty(() => Exercise, value); }
        }

        //public bool Empty => Exercise == null;
    }
}