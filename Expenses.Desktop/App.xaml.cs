using System.Windows;
using DevExpress.Xpf.Core;

namespace Expenses.UI
{
    public partial class App : Application
    {
        private void OnAppStartup(object sender, StartupEventArgs e)
        {
            ApplicationThemeHelper.UpdateApplicationThemeName();
        }
    }
}