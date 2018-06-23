using System;
using System.Collections.Generic;
using System.Configuration;
using System.Windows;

namespace XGamer.UI.Loader
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            e.Handled = true;
            ErrorWindow error = new ErrorWindow(e.Exception);
            try
            {
                error.Owner = Application.Current.MainWindow;
            }
            catch
            {
                error.Owner = null;
            }

            error.ShowDialog();

            if (error.Owner == null)
            {
                Application.Current.Shutdown(0);
            }
        }
    }
}
