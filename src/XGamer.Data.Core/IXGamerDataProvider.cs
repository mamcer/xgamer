using System.Collections.Generic;
using XGamer.Data.Entities;

namespace XGamer.Data.Core
{
    public interface IXGamerDataProvider
    {
        IEnumerable<Rom> GetAllGames();
        
        IEnumerable<Emulator> GetAllEmulators();

        Rom GetGameById(int gameId);
    }
}