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
            this.InitializeComponent();

            this.InititalizeTimer();

            IEnumerable<Game> allGames = XGamerEngine.Instance.GetAllGames();
            this.lstGames.Items.Clear();
            foreach (Game game in allGames)
            {
                this.lstGames.Items.Add(new ListBoxItem() { Content = game.GameName, Uid = game.Id.ToString() });
            }

            this.lstGames.Focus();
            if (this.lstGames.HasItems)
            {
                this.lstGames.SelectedIndex = 0;
            }

            this.Worker.DoWork += new DoWorkEventHandler(this.worker_DoWork);

            this.lblGameCount.Content = string.Format("Total {0} Juegos", allGames.Count());
        }

        public void SetPosterImage(BitmapImage source)
        {
            this.imgPoster.Source = source.Clone();
        }

        public void SetInGameImage(BitmapImage source)
        {
            this.imgInGame.Source = source.Clone();
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
            if (MessageBox.Show("Esto cerrará la Aplicación. Esta seguro?", this.Title, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
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
                try
                {
                    int gameId = Convert.ToInt32(selectedGame.Uid);
                    XGamerEngine.Instance.PlayGame(gameId);
                }
                finally
                {
                    this.IsEnabled = true;
                }
            }
        }

        private void lstGames_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBoxItem selectedGame = this.lstGames.SelectedItem as ListBoxItem;
            if (selectedGame != null)
            {
                int gameId = Convert.ToInt32(selectedGame.Uid);


                BitmapImage bi = XGamerEngine.Instance.GetGamePosterById(gameId);
                if (bi != null)
                {
                    this.imgPoster.Dispatcher.BeginInvoke((Action)delegate() { this.imgPoster.Source = bi; });
                }

                BitmapImage bi2 = XGamerEngine.Instance.GetGameInGamePosterById(gameId);
                if (bi2 != null)
                {
                    this.imgInGame.Dispatcher.BeginInvoke((Action)delegate() { this.imgInGame.Source = bi2; });
                }
            }
        }

        private void lblMinimize_MouseEnter(object sender, MouseEventArgs e)
        {
            this.lblMinimize.Foreground = System.Windows.Media.Brushes.Black;
        }

        private void lblMinimize_MouseLeave(object sender, MouseEventArgs e)
        {
            this.lblMinimize.Foreground = System.Windows.Media.Brushes.White;
        }

        private void lblClose_MouseEnter(object sender, MouseEventArgs e)
        {
            this.lblClose.Foreground = System.Windows.Media.Brushes.Black;
        }

        private void lblClose_MouseLeave(object sender, MouseEventArgs e)
        {
            this.lblClose.Foreground = System.Windows.Media.Brushes.White;
        }
    }
}
