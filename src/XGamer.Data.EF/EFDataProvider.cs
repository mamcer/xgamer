using System.Collections.Generic;
using System.Linq;

using XGamer.Data.Core;
using XGamer.Data.Entities;

namespace XGamer.Data.EF
{
    public class EFDataProvider : IXGamerDataProvider
    {
        #region private fields

        private static EFDataProvider instance;
        private static object lockObject = new object();

        #endregion

        #region constructor

        private EFDataProvider()
        {
        }

        #endregion

        #region private properties

        public static EFDataProvider Instance
        {
            get
            {
                lock (lockObject)
                {
                    if (instance == null)
                    {
                        instance = new EFDataProvider();
                    }

                    return instance;
                }
            }
        }

        #endregion

        #region public methods

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
        
        #endregion
    }
}
