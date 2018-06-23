using System.IO;
using System.Windows;

namespace XGamer.UI.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        internal static JoysticksWindow joystickMessageWindow;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            if (!File.Exists("startupmessage"))
            {
                joystickMessageWindow = new JoysticksWindow();
                joystickMessageWindow.Show();
            }
        }

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
