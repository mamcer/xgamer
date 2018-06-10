using System.Collections.Generic;
using System.Linq;

using XGamer.Data.Core;
using XGamer.Data.Entities;

namespace XGamer.Data.EF
{
    public class EFDataProvider : IXGamerDataProvider
    {
        private static EFDataProvider _instance;
        private static readonly object LockObject = new object();

        private EFDataProvider()
        {
        }
        
        public static EFDataProvider Instance
        {
            get
            {
                lock (LockObject)
                {
                    if (_instance == null)
                    {
                        _instance = new EFDataProvider();
                    }

                    return _instance;
                }
            }
        }

        public IEnumerable<Game> GetAllGames()
        {
            XGamerEntities xgamerEntities = new XGamerEntities();
            return xgamerEntities.Game.ToList().OrderBy(x => x.GameName);
        }

        public IEnumerable<Emulator> GetAllEmulators()
        {
            XGamerEntities xgamerEntities = new XGamerEntities();
            return xgamerEntities.Emulator.ToList();
        }

        public Game GetGameById(int gameId)
        {
            XGamerEntities xgamerEntities = new XGamerEntities();
            return xgamerEntities.Game.FirstOrDefault(x => x.Id == gameId);
        }
    }
}