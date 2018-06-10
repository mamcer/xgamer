using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using XGamer.Core;
using XGamer.Data.Entities;

namespace XGamer.UI.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DispatcherTimer timer;
        private BackgroundWorker worker;

        public MainWindow()
        {
            InitializeComponent();

            InititalizeTimer();

            IEnumerable<Game> allGames = XGamerEngine.Instance.GetAllGames();
            lstGames.Items.Clear();
            foreach (Game game in allGames)
            {
                lstGames.Items.Add(new ListBoxItem() { Content = game.GameName, Uid = game.Id.ToString() });
            }

            lstGames.Focus();
            if (lstGames.HasItems)
            {
                lstGames.SelectedIndex = 0;
            }

            Worker.DoWork += new DoWorkEventHandler(worker_DoWork);

            lblGameCount.Content = string.Format("Total {0} Juegos", allGames.Count());
        }

        public void SetPosterImage(BitmapImage source)
        {
            imgPoster.Source = source.Clone();
        }

        public void SetInGameImage(BitmapImage source)
        {
            imgInGame.Source = source.Clone();
        }

        private BackgroundWorker Worker
        {
            get
            {
                if (worker == null)
                {
                    worker = new BackgroundWorker();
                }

                return worker;
            }
        }

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            int gameId = Convert.ToInt32(e.Argument);

            imgPoster.Dispatcher.BeginInvoke((Action)(() =>
            {
                BitmapImage bi = XGamerEngine.Instance.GetGamePosterById(gameId);
                if (bi != null)
                {
                    imgPoster.Source = bi;
                }
            }));

            imgInGame.Dispatcher.BeginInvoke((Action)(() =>
            {
                BitmapImage bi2 = XGamerEngine.Instance.GetGameInGamePosterById(gameId);
                if (bi2 != null)
                {
                    imgInGame.Source = bi2;
                }
            }));
        }

        private void InititalizeTimer()
        {
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Tick += new EventHandler(Timer_Tick);
            timer.IsEnabled = true;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            lblCurrentTime.Content = string.Format("{0:00}:{1:00}", DateTime.Now.Hour, DateTime.Now.Minute);
        }

        private void LstGames_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            RunGame();
        }

        private void LblMinimize_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void LblClose_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (MessageBox.Show("Esto cerrará la Aplicación. Esta seguro?", Title, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                Close();
            }
        }

        private void LstGames_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                RunGame();
            }
        }

        private void RunGame()
        {
            ListBoxItem selectedGame = lstGames.SelectedItem as ListBoxItem;
            if (selectedGame != null)
            {
                IsEnabled = false;
                try
                {
                    int gameId = Convert.ToInt32(selectedGame.Uid);
                    XGamerEngine.Instance.PlayGame(gameId);
                }
                finally
                {
                    IsEnabled = true;
                }
            }
        }

        private void lstGames_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBoxItem selectedGame = lstGames.SelectedItem as ListBoxItem;
            if (selectedGame != null)
            {
                int gameId = Convert.ToInt32(selectedGame.Uid);


                BitmapImage bi = XGamerEngine.Instance.GetGamePosterById(gameId);
                if (bi != null)
                {
                    imgPoster.Dispatcher.BeginInvoke((Action)delegate() { imgPoster.Source = bi; });
                }

                BitmapImage bi2 = XGamerEngine.Instance.GetGameInGamePosterById(gameId);
                if (bi2 != null)
                {
                    imgInGame.Dispatcher.BeginInvoke((Action)delegate() { imgInGame.Source = bi2; });
                }
            }
        }

        private void lblMinimize_MouseEnter(object sender, MouseEventArgs e)
        {
            lblMinimize.Foreground = System.Windows.Media.Brushes.Black;
        }

        private void lblMinimize_MouseLeave(object sender, MouseEventArgs e)
        {
            lblMinimize.Foreground = System.Windows.Media.Brushes.White;
        }

        private void lblClose_MouseEnter(object sender, MouseEventArgs e)
        {
            lblClose.Foreground = System.Windows.Media.Brushes.Black;
        }

        private void lblClose_MouseLeave(object sender, MouseEventArgs e)
        {
            lblClose.Foreground = System.Windows.Media.Brushes.White;
        }
    }
}
