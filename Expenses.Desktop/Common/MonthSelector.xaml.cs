using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Editors.DateNavigator;
using DevExpress.Xpf.Editors.DateNavigator.Controls;

namespace Expenses.UI.Common
{
    public partial class MonthSelector : DateNavigator
    {
        public MonthSelector()
        {
            InitializeComponent();
        }

        protected override void OnCalendarButtonClick(object sender, DateNavigatorCalendarButtonClickEventArgs e)
        {
            base.OnCalendarButtonClick(sender, e);

            var editor = GetValue(BaseEdit.OwnerEditProperty) as PopupBaseEdit;
            if (CalendarView == DateNavigatorCalendarView.Year) editor?.ClosePopup();
        }
    }
}