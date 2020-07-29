using Redox.API.Console;
using Redox.API.Player;
using Redox.Core.Commands;
using Redox.Core.User;

namespace Redox.API.Commands
{
    /// <summary>
    /// Represents a user that executes a command.
    /// </summary>
    public sealed class CommandExecutor : ICommandExecutor
    {
        /// <summary>
        /// The user that executes the command.
        /// <para>i.e. IPlayer, IConsole</para>
        /// </summary>
        public IUser User { get; }

        /// <summary>
        /// Gets if the executer is an player.
        /// </summary>
        public bool IsPlayer
        {
            get
            {
                return User is IPlayer;
            }
        }

        /// <summary>
        /// Gets if the executor is an console.
        /// </summary>
        public bool IsConsole
        {
            get
            {
                return User is IConsole;
            }
        }

        /// <summary>
        /// Returns the console user that executed the command.
        /// </summary>
        /// <returns></returns>
        public IConsole GetConsole()
        {
            return User as IConsole;
        }

        /// <summary>
        /// Returns the player that executed the command.
        /// </summary>
        /// <returns></returns>
        public IPlayer GetPlayer()
        {
            return User as IPlayer;
        }

        public CommandExecutor(IUser user)
        {
            this.User = user;
        }
    }
}
