using System.Globalization;
using System.Threading;
using System.Windows;
using DevExpress.Xpf.Core;

namespace Expenses.UI
{
    public partial class App : Application
    {
        private void OnAppStartup(object sender, StartupEventArgs e)
        {
            ApplicationThemeHelper.UpdateApplicationThemeName();

            var culture = CultureInfo.CreateSpecificCulture("fr-FR");
            Thread.CurrentThread.CurrentUICulture = culture;
            Thread.CurrentThread.CurrentCulture = culture;
        }
    }
}