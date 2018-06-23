using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using XGamer.Core;
using XGamer.Data.Entities;

namespace XGamer.UI.WPF
{
    public partial class MainWindow : IDisposable
    {
        private DispatcherTimer _timer;
        private BackgroundWorker _worker;

        public MainWindow()
        {
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            InitializeComponent();

            InititalizeTimer();

            var allGames = XGamerEngine.Instance.GetAllGames();
            lstGames.Items.Clear();
            var games = allGames as Rom[] ?? allGames.ToArray();
            foreach (Rom game in games)
            {
                lstGames.Items.Add(new ListBoxItem() {Content = game.GameName, Uid = game.Id.ToString()});
            }

            lstGames.Focus();
            if (lstGames.HasItems)
            {
                lstGames.SelectedIndex = 0;
            }

            Worker.DoWork += Worker_DoWork;

            lblGameCount.Content = $"{games.Length} Juegos";

            grdHeader.Background = new SolidColorBrush(XGamerEnvironment.BackgroundColor);
            grdFooter.Background = new SolidColorBrush(XGamerEnvironment.BackgroundColor);
        }

        private BackgroundWorker Worker => _worker ?? (_worker = new BackgroundWorker());

        public void SetPosterImage(BitmapImage source)
        {
            imgPoster.Source = source.Clone();
        }

        public void SetInGameImage(BitmapImage source)
        {
            imgInGame.Source = source.Clone();
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
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
            _timer = new DispatcherTimer {Interval = new TimeSpan(0, 0, 1)};
            _timer.Tick += Timer_Tick;
            _timer.IsEnabled = true;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            lblCurrentTime.Content = $"{DateTime.Now.Hour:00}:{DateTime.Now.Minute:00}";
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
            CloseApplication closeApplication = new CloseApplication {Owner = this};
            if (closeApplication.ShowDialog() == true)
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
            if (lstGames.SelectedItem is ListBoxItem selectedGame)
            {
                IsEnabled = false;
                LoadingGame loadingGame = new LoadingGame {Owner = this};

                if (App.joystickMessageWindow != null)
                {
                    App.joystickMessageWindow.Hide();
                }

                loadingGame.Show();
                try
                {
                    int gameId = Convert.ToInt32(selectedGame.Uid);
                    XGamerEngine.Instance.PlayGame(gameId);
                }
                finally
                {
                    loadingGame.Close();
                    IsEnabled = true;
                }
            }
        }

        private void LstGames_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstGames.SelectedItem is ListBoxItem selectedGame)
            {
                int gameId = Convert.ToInt32(selectedGame.Uid);

                BitmapImage bi = XGamerEngine.Instance.GetGamePosterById(gameId);
                if (bi != null)
                {
                    imgPoster.Dispatcher.BeginInvoke((Action)delegate { imgPoster.Source = bi; });
                }

                BitmapImage bi2 = XGamerEngine.Instance.GetGameInGamePosterById(gameId);
                if (bi2 != null)
                {
                    imgInGame.Dispatcher.BeginInvoke((Action)delegate { imgInGame.Source = bi2; });
                }
            }
        }

        private void LblMinimize_MouseEnter(object sender, MouseEventArgs e)
        {
            lblMinimize.Foreground = Brushes.Black;
        }

        private void LblMinimize_MouseLeave(object sender, MouseEventArgs e)
        {
            lblMinimize.Foreground = Brushes.White;
        }

        private void LblClose_MouseEnter(object sender, MouseEventArgs e)
        {
            lblClose.Foreground = Brushes.Black;
        }

        private void LblClose_MouseLeave(object sender, MouseEventArgs e)
        {
            lblClose.Foreground = Brushes.White;
        }

        protected virtual void Dispose(bool b)
        {
            if (b)
            {
                _worker.Dispose();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}