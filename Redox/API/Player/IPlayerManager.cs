using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Redox.API.Player
{
    public interface IPlayerManager 
    {
        /// <summary>
        /// Finds a player by name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        IPlayer FindPlayer(string name);

        /// <summary>
        /// Finds a player by id
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        IPlayer FindPlayerByID(string ID);

    }
}
