using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Redox.API.Player
{
    public interface IPlayerManager
    {
        IEnumerable<IPlayer> GetPlayers();

        /// <summary>
        /// Finds a player by name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<IPlayer> FindPlayerAsync(string name);

        /// <summary>
        /// Finds a player by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<IPlayer> FindPlayerByIdAsync(string id);
    }
}
