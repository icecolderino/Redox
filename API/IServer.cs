using System;
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
        int ServerPort { get; }
        
        /// <summary>
        /// The app id of the game.
        /// </summary>
        int AppId { get; }

        /// <summary>
        /// The game version
        /// </summary>
        Version GameVersion { get; }

        /// <summary>
        /// The culture of the server.
        /// </summary>
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

        /// <summary>
        /// Broadcasts a global message in chat.
        /// </summary>
        /// <param name="message">The message you want to send.</param>
        void Broadcast(string message);

        /// <summary>
        /// Broadcasts a global message in chat with prefix.
        /// </summary>
        /// <param name="prefix">The prefix of the message.</param>
        /// <param name="message">The message you want to send.</param>
        void Broadcast(string prefix, string message);

        /// <summary>
        /// Unban a player from the server
        /// </summary>
        /// <param name="id">The id of the player.</param>
        void Unban(string id);

    }
}
   