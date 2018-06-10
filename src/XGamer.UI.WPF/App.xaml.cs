using System.Windows;

namespace XGamer.UI.WPF
{
    public partial class App : Application
    {
        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            e.Handled = true;
            ErrorWindow error = new ErrorWindow(e.Exception);
            try
            {
                error.Owner = Current.MainWindow;
            }
            catch
            {
                error.Owner = null;
            }

            error.ShowDialog();
            
            if (error.Owner == null)
            {
                Current.Shutdown(0);
            }
        }
    }
}