using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;
using System.ComponentModel;

using XGamer.Data.Core;
using XGamer.Data.Entities;

namespace XGamer.Core
{
    public class XGamerEngine
    {
        private IXGamerDataProvider dataProvider;
        private IEnumerable<Emulator> emulators;
        private IEnumerable<Game> games;
        private ImageCache imageCache;
        private static XGamerEngine instance;

        private XGamerEngine()
        {
            imageCache = new ImageCache(XGamerEnvironment.PicturesPath);
            imageCache.ProcessImageFolder();
        }

        public static XGamerEngine Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new XGamerEngine();
                }

                return instance;
            }
        }

        private IXGamerDataProvider DataProvider
        {
            get
            {
                if (this.dataProvider == null)
                {
                    this.dataProvider = XGamerDataProviderFactory.GetDataProvider(XGamerDataProviderType.EntityFramework);
                }

                return this.dataProvider;
            }
        }

        private IEnumerable<Game> Games
        {
            get
            {
                if (this.games == null)
                {
                    this.games = this.DataProvider.GetAllGames();
                }

                return this.games;
            }
        }

        private IEnumerable<Emulator> Emulators
        {
            get
            {
                if (this.emulators == null)
                {
                    this.emulators = this.DataProvider.GetAllEmulators();
                }

                return this.emulators;
            }
        }

        public IEnumerable<Game> GetAllGames()
        {
            return this.Games;
        }

        public bool PlayGame(int gameId)
        {
            Game game = this.GetGameById(gameId);
            if (game != null)
            {
                Emulator emulator = this.Emulators.ToList().Find(x => x.RomType == game.Type);
                if (emulator != null)
                {
                    return EmulatorManager.Instance.RunGame(emulator, game);
                }
            }
        
            return false;
        }

        public Game GetGameById(int gameId)
        {
            return this.DataProvider.GetGameById(gameId);
        }

        public BitmapImage GetGamePosterById(int gameId)
        {
            Game game = this.DataProvider.GetGameById(gameId);

            string fileName = game.GamePoster;
            BitmapImage bi = null;
            bi = this.imageCache.GetImage(fileName);
            if (bi == null)
            {
                bi = new BitmapImage();
                try
                {
                    string posterPath = Path.Combine(XGamerEnvironment.PicturesPath, game.RomType.Description);
                    bi.BeginInit();
                    bi.CacheOption = BitmapCacheOption.OnLoad;
                    bi.UriSource = new Uri(Path.Combine(posterPath, game.GamePoster));
                    bi.EndInit();
                }
                catch
                {
                    bi = null;
                }
            }

            return bi;
        }


        public BitmapImage GetGameInGamePosterById(int gameId)
        {
            Game game = this.DataProvider.GetGameById(gameId);

            string fileName = game.InGamePoster;
            BitmapImage bi = null;

            bi = this.imageCache.GetImage(fileName);
            if (bi == null)
            {
                bi = new BitmapImage();
                try
                {
                    string posterPath = Path.Combine(XGamerEnvironment.PicturesPath, game.RomType.Description);
                    bi.BeginInit();
                    bi.CacheOption = BitmapCacheOption.OnLoad;
                    bi.UriSource = new Uri(Path.Combine(posterPath, game.InGamePoster));
                    bi.EndInit();
                }
                catch
                {
                    bi = null;
                }
            }

            return bi;
        }
    }
}
