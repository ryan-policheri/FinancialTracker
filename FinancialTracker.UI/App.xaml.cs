using System;
using System.Windows;
using System.Windows.Threading;
using Autofac;
using FinancialTracker.UI.Startup;

namespace FinancialTracker.UI
{
    public partial class App : Application
    {
        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            var bootstrapper = new Bootstrapper();
            var container = bootstrapper.Bootstrap();

            var mainWindow = container.Resolve<MainWindow>();
            mainWindow.Show();
        }

        private void App_OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show(
                "Unexpected Error Occurred. Please inform the admin." + Environment.NewLine + e.Exception.Message,
                "Unexpected Error");

            e.Handled = true;
        }
    }
}
