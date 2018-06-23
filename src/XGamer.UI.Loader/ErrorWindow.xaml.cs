using System;
using System.Windows;

namespace XGamer.UI.Loader
{
    /// <summary>
    /// Interaction logic for ErrorWindow.xaml
    /// </summary>
    public partial class ErrorWindow : Window
    {
        public ErrorWindow(Exception ex)
        {
            this.InitializeComponent();

            if (ex != null)
            {
                this.txtError.Text = ex.Message;
            }
        }

        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Label_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Clipboard.SetText(this.txtError.Text);
        }
    }
}
