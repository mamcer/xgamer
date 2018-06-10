using System;
using System.Windows;

namespace XGamer.UI.WPF
{
    public partial class ErrorWindow : Window
    {
        public ErrorWindow(Exception ex)
        {
            InitializeComponent();

            if (ex != null)
            {
                txtError.Text = ex.Message;
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