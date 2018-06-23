using System.Windows;
using System.Windows.Media;
using XGamer.Core;

namespace XGamer.UI.WPF
{
    /// <summary>
    /// Interaction logic for CloseApplication.xaml
    /// </summary>
    public partial class CloseApplication : Window
    {
        public CloseApplication()
        {
            InitializeComponent();
            this.grdContent.Background = new SolidColorBrush(XGamerEnvironment.BackgroundColor);
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
