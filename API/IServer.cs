﻿using System;
using System.Globalization;
using System.Threading.Tasks;
using Redox.API.Console;
using Redox.API.Player;

namespace Redox.API
{
    public interface IServer : IPlayerManager
    {
        /// <summary>
        /// The name of the server
        /// </summary>
        string ServerName { get; set; }

        /// <summary>
        /// The name of the game
        /// </summary>
        string GameName { get; }

        /// <summary>
        /// The server port
        /// </summary>
        ushort ServerPort { get; }

        /// <summary>
        /// The game version
        /// </summary>
        Version GameVersion { get; }

        CultureInfo Language { get; }
        /// <summary>
        /// The console of the game server
        /// </summary>
        IConsole Console { get; }

        /// <summary>
        /// Reloads RedoxMod with all the plugins
        /// </summary>
        /// <returns></returns>
        void Reload();

        /// <summary>
        /// Unloads all plugins and shutdowns the server
        /// </summary>
        /// <returns></returns>
        void Shutdown();


        void Broadcast(string message);

        void Broadcast(string prefix, string message);

        /// <summary>
        /// Unban a player from the server
        /// </summary>
        /// <param name="id"></param>
        void Unban(string id);

    }
}
   