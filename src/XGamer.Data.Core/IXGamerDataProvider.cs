using System.Collections.Generic;

using XGamer.Data.Entities;

namespace XGamer.Data.Core
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IXGamerDataProvider
    {
        IEnumerable<Game> GetAllGames();
        
        IEnumerable<Emulator> GetAllEmulators();

        Game GetGameById(int gameId);
    }
}
