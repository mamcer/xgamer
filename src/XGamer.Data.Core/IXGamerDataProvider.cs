using System.Collections.Generic;
using XGamer.Data.Entities;

namespace XGamer.Data.Core
{
    public interface IXGamerDataProvider
    {
        IEnumerable<Game> GetAllGames();
        
        IEnumerable<Emulator> GetAllEmulators();

        Game GetGameById(int gameId);
    }
}