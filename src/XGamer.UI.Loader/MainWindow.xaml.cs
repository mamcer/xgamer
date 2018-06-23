using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Media;

namespace XGamer.UI.Loader
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
            if (this.IsAlreadyInstalled())
            {
                this.btnFromPC.Background = new SolidColorBrush(new Color() { R = 128, G = 255, B = 128, A = 255 });
                this.btnFromPC.Content = XGamer.UI.Loader.Properties.Resources.PlayFromPC;
            }
        }

        private void BtnFromDVD_Click(object sender, RoutedEventArgs e)
        {
            Directory.SetCurrentDirectory(Path.Combine(".", XGamer.UI.Loader.Properties.Resources.XGamerSourceDirectory));
            Process.Start(XGamer.UI.Loader.Properties.Resources.XGamerTargetName);
            this.Close();
        }

        private bool IsAlreadyInstalled()
        {
            if (Directory.Exists(XGamer.UI.Loader.Properties.Resources.XGamerTargetDirectory))
            {
                return File.Exists(Path.Combine(XGamer.UI.Loader.Properties.Resources.XGamerTargetDirectory, Path.Combine(XGamer.UI.Loader.Properties.Resources.XGamerTargetName)));
            }

            return false;
        }
        
        private void BtnFromPC_Click(object sender, RoutedEventArgs e)
        {
            if (!this.IsAlreadyInstalled())
            {
                Directory.CreateDirectory(XGamer.UI.Loader.Properties.Resources.XGamerTargetDirectory);
                string sourcePath = XGamer.UI.Loader.Properties.Resources.XGamerSourceDirectory;
                string destinationPath = XGamer.UI.Loader.Properties.Resources.XGamerTargetDirectory;
                CopyInProgressWindow copyWindow = new CopyInProgressWindow(sourcePath, destinationPath);
               
                if (copyWindow.ShowDialog() != true)
                {
                    return;
                }
            }

            Directory.SetCurrentDirectory(XGamer.UI.Loader.Properties.Resources.XGamerTargetDirectory);
            Process.Start(Path.Combine(XGamer.UI.Loader.Properties.Resources.XGamerTargetDirectory, Path.Combine(XGamer.UI.Loader.Properties.Resources.XGamerTargetName)));
            this.Close();
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
