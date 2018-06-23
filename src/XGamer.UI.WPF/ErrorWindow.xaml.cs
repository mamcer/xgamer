using System;
using System.Windows;
using System.Windows.Media;
using XGamer.Core;

namespace XGamer.UI.WPF
{
    /// <summary>
    /// Interaction logic for ErrorWindow.xaml
    /// </summary>
    public partial class ErrorWindow : Window
    {
        public ErrorWindow(Exception ex)
        {
            InitializeComponent();

            grdContent.Background = new SolidColorBrush(XGamerEnvironment.BackgroundColor);

            if (ex != null)
            {
                txtError.Text = ex.Message;
                if (ex.InnerException != null)
                {
                    txtError.Text += " - " + ex.Message;
                }
            }
        }

        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Label_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Clipboard.SetText(txtError.Text);
        }
    }
}
