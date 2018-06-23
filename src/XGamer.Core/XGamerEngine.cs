using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Media.Imaging;

using XGamer.Data.Core;
using XGamer.Data.Entities;

namespace XGamer.Core
{
    public class XGamerEngine
    {
        private static XGamerEngine instance;

        private IXGamerDataProvider dataProvider;
        private IEnumerable<Emulator> emulators;
        private IEnumerable<Rom> games;
        private ImageCache imageCache;
        
        private XGamerEngine()
        {
            this.imageCache = new ImageCache(XGamerEnvironment.PicturesPath);
            this.imageCache.ProcessImageFolder();
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
                    this.dataProvider = XGamerDataProviderFactory.GetDataProvider(XGamerDataProviderType.Xml);
                }

                return this.dataProvider;
            }
        }

        private IEnumerable<Rom> Games
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

        public IEnumerable<Rom> GetAllGames()
        {
            return this.Games;
        }

        public bool PlayGame(int gameId)
        {
            Rom game = this.GetGameById(gameId);
            if (game != null)
            {
                Emulator emulator = this.Emulators.ToList().Find(x => x.IdRomType == game.IdRomType);
                if (emulator != null)
                {
                    return EmulatorManager.Instance.RunGame(emulator, game);
                }
            }
        
            return false;
        }

        public Rom GetGameById(int gameId)
        {
            return this.DataProvider.GetGameById(gameId);
        }

        public BitmapImage GetGamePosterById(int gameId)
        {
            Rom game = this.DataProvider.GetGameById(gameId);

            string fileName = game.Poster1FileName;
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
                    bi.UriSource = new Uri(Path.Combine(posterPath, game.Poster1FileName));
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
            Rom game = this.DataProvider.GetGameById(gameId);

            string fileName = game.Poster2FileName;
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
                    bi.UriSource = new Uri(Path.Combine(posterPath, game.Poster2FileName));
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
