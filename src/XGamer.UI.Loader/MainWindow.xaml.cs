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
            InitializeComponent();
            if (IsAlreadyInstalled())
            {
                btnFromPC.Background = new SolidColorBrush(new Color() { R = 128, G = 255, B = 128, A = 255 });
                btnFromPC.Content = Properties.Resources.PlayFromPC;
            }
        }

        private void BtnFromDVD_Click(object sender, RoutedEventArgs e)
        {
            Directory.SetCurrentDirectory(Path.Combine(".", Properties.Resources.XGamerSourceDirectory));
            Process.Start(Properties.Resources.XGamerTargetName);
            Close();
        }

        private bool IsAlreadyInstalled()
        {
            if (Directory.Exists(Properties.Resources.XGamerTargetDirectory))
            {
                return File.Exists(Path.Combine(Properties.Resources.XGamerTargetDirectory, Path.Combine(Properties.Resources.XGamerTargetName)));
            }

            return false;
        }
        
        private void BtnFromPC_Click(object sender, RoutedEventArgs e)
        {
            if (!IsAlreadyInstalled())
            {
                Directory.CreateDirectory(Properties.Resources.XGamerTargetDirectory);
                string sourcePath = Properties.Resources.XGamerSourceDirectory;
                string destinationPath = Properties.Resources.XGamerTargetDirectory;
                CopyInProgressWindow copyWindow = new CopyInProgressWindow(sourcePath, destinationPath);
               
                if (copyWindow.ShowDialog() != true)
                {
                    return;
                }
            }

            Directory.SetCurrentDirectory(Properties.Resources.XGamerTargetDirectory);
            Process.Start(Path.Combine(Properties.Resources.XGamerTargetDirectory, Path.Combine(Properties.Resources.XGamerTargetName)));
            Close();
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}