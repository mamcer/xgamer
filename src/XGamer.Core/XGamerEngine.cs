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
        private IXGamerDataProvider _dataProvider;
        private IEnumerable<Emulator> _emulators;
        private IEnumerable<Game> _games;
        private readonly ImageCache _imageCache;
        private static XGamerEngine _instance;

        private XGamerEngine()
        {
            _imageCache = new ImageCache(XGamerEnvironment.PicturesPath);
            _imageCache.ProcessImageFolder();
        }

        public static XGamerEngine Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new XGamerEngine();
                }

                return _instance;
            }
        }

        private IXGamerDataProvider DataProvider
        {
            get
            {
                if (_dataProvider == null)
                {
                    _dataProvider = XGamerDataProviderFactory.GetDataProvider(XGamerDataProviderType.EntityFramework);
                }

                return _dataProvider;
            }
        }

        private IEnumerable<Game> Games
        {
            get
            {
                if (_games == null)
                {
                    _games = DataProvider.GetAllGames();
                }

                return _games;
            }
        }

        private IEnumerable<Emulator> Emulators
        {
            get
            {
                if (_emulators == null)
                {
                    _emulators = DataProvider.GetAllEmulators();
                }

                return _emulators;
            }
        }

        public IEnumerable<Game> GetAllGames()
        {
            return Games;
        }

        public bool PlayGame(int gameId)
        {
            Game game = GetGameById(gameId);
            if (game != null)
            {
                Emulator emulator = Emulators.ToList().Find(x => x.RomType == game.Type);
                if (emulator != null)
                {
                    return EmulatorManager.Instance.RunGame(emulator, game);
                }
            }
        
            return false;
        }

        public Game GetGameById(int gameId)
        {
            return DataProvider.GetGameById(gameId);
        }

        public BitmapImage GetGamePosterById(int gameId)
        {
            Game game = DataProvider.GetGameById(gameId);

            string fileName = game.GamePoster;
            var bi = _imageCache.GetImage(fileName);
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
            Game game = DataProvider.GetGameById(gameId);

            string fileName = game.InGamePoster;

            var bi = _imageCache.GetImage(fileName);
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