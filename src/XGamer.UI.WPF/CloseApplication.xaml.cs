using System.Windows;
using System.Windows.Media;
using XGamer.Core;

namespace XGamer.UI.WPF
{
    public partial class CloseApplication : Window
    {
        public CloseApplication()
        {
            InitializeComponent();
            grdContent.Background = new SolidColorBrush(XGamerEnvironment.BackgroundColor);
        }

        private void Button2_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}