using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Redox.API.Player
{
    public interface IPlayerManager 
    {
        /// <summary>
        /// Returns all online players in the server
        /// </summary>
        IEnumerable<IPlayer> Players { get; }

        /// <summary>
        /// Finds a player by name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<IPlayer> FindPlayer(string name);

        /// <summary>
        /// Finds a player by id
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        Task<IPlayer> FindPlayerByID(string ID);



    }
}
