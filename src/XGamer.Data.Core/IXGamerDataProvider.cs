using System.Collections.Generic;

using XGamer.Data.Entities;

namespace XGamer.Data.Core
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IXGamerDataProvider
    {
        IEnumerable<Rom> GetAllGames();
        
        IEnumerable<Emulator> GetAllEmulators();

        Rom GetGameById(int gameId);
    }
}
