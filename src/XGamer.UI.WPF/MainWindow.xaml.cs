using System;
using System.Collections.Generic;
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
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IDisposable
    {
        private DispatcherTimer timer;
        private BackgroundWorker worker;

        public MainWindow()
        {
            this.Loaded += new RoutedEventHandler(MainWindow_Loaded);
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.InitializeComponent();

            this.InititalizeTimer();

            IEnumerable<Rom> allGames;
            allGames = XGamerEngine.Instance.GetAllGames();
            this.lstGames.Items.Clear();
            foreach (Rom game in allGames)
            {
                this.lstGames.Items.Add(new ListBoxItem() {Content = game.GameName, Uid = game.Id.ToString()});
            }

            this.lstGames.Focus();
            if (this.lstGames.HasItems)
            {
                this.lstGames.SelectedIndex = 0;
            }

            this.Worker.DoWork += new DoWorkEventHandler(this.Worker_DoWork);

            this.lblGameCount.Content = string.Format("{0} Juegos", allGames.Count());

            this.grdHeader.Background = new SolidColorBrush(XGamerEnvironment.BackgroundColor);
            this.grdFooter.Background = new SolidColorBrush(XGamerEnvironment.BackgroundColor);
        }

        private BackgroundWorker Worker
        {
            get
            {
                if (this.worker == null)
                {
                    this.worker = new BackgroundWorker();
                }

                return this.worker;
            }
        }

        public void SetPosterImage(BitmapImage source)
        {
            this.imgPoster.Source = source.Clone();
        }

        public void SetInGameImage(BitmapImage source)
        {
            this.imgInGame.Source = source.Clone();
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            int gameId = Convert.ToInt32(e.Argument);

            this.imgPoster.Dispatcher.BeginInvoke((Action)(() =>
            {
                BitmapImage bi = XGamerEngine.Instance.GetGamePosterById(gameId);
                if (bi != null)
                {
                    imgPoster.Source = bi;
                }
            }));

            this.imgInGame.Dispatcher.BeginInvoke((Action)(() =>
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
            this.timer = new DispatcherTimer();
            this.timer.Interval = new TimeSpan(0, 0, 1);
            this.timer.Tick += new EventHandler(this.Timer_Tick);
            this.timer.IsEnabled = true;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            this.lblCurrentTime.Content = string.Format("{0:00}:{1:00}", DateTime.Now.Hour, DateTime.Now.Minute);
        }

        private void LstGames_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            this.RunGame();
        }

        private void LblMinimize_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void LblClose_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            CloseApplication closeApplication = new CloseApplication();
            closeApplication.Owner = this;
            if (closeApplication.ShowDialog() == true)
            {
                this.Close();
            }
        }

        private void LstGames_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                this.RunGame();
            }
        }

        private void RunGame()
        {
            ListBoxItem selectedGame = this.lstGames.SelectedItem as ListBoxItem;
            if (selectedGame != null)
            {
                this.IsEnabled = false;
                LoadingGame loadingGame = new LoadingGame();
                loadingGame.Owner = this;
                
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
                    this.IsEnabled = true;
                }
            }
        }

        private void LstGames_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBoxItem selectedGame = this.lstGames.SelectedItem as ListBoxItem;
            if (selectedGame != null)
            {
                int gameId = Convert.ToInt32(selectedGame.Uid);

                BitmapImage bi = XGamerEngine.Instance.GetGamePosterById(gameId);
                if (bi != null)
                {
                    this.imgPoster.Dispatcher.BeginInvoke((Action)delegate { this.imgPoster.Source = bi; });
                }

                BitmapImage bi2 = XGamerEngine.Instance.GetGameInGamePosterById(gameId);
                if (bi2 != null)
                {
                    this.imgInGame.Dispatcher.BeginInvoke((Action)delegate { this.imgInGame.Source = bi2; });
                }
            }
        }

        private void LblMinimize_MouseEnter(object sender, MouseEventArgs e)
        {
            this.lblMinimize.Foreground = System.Windows.Media.Brushes.Black;
        }

        private void LblMinimize_MouseLeave(object sender, MouseEventArgs e)
        {
            this.lblMinimize.Foreground = System.Windows.Media.Brushes.White;
        }

        private void LblClose_MouseEnter(object sender, MouseEventArgs e)
        {
            this.lblClose.Foreground = System.Windows.Media.Brushes.Black;
        }

        private void LblClose_MouseLeave(object sender, MouseEventArgs e)
        {
            this.lblClose.Foreground = System.Windows.Media.Brushes.White;
        }

        protected virtual void Dispose(bool b)
        {
            if (b)
            {
                worker.Dispose();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
