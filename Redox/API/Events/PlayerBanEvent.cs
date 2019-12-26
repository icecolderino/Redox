using System;

using Redox.API.Player;
using Redox.API.Console;
using Redox.Core.User;

namespace Redox.API.Events
{
    /// <summary>
    /// Generic event when a player gets banned, This class cannot be inherit
    /// </summary>
    public sealed class PlayerBanEvent
    {
        /// <summary>
        /// The reason for the ban
        /// </summary>
        public string Reason { get; private set; }

        /// <summary>
        /// The player that got banned
        /// </summary>
        public IPlayer Victim { get; private set; }

        /// <summary>
        /// The Executor of the ban command, This either be an console or player
        /// </summary>
        public IUser Executor { get; private set; }

        /// <summary>
        /// Gets if the Executor is an player
        /// </summary>
        /// <returns></returns>
        public bool ExecutorIsPlayer()
        {
            return Executor is IPlayer;
        }
        /// <summary>
        /// Gets if the Executor is the player
        /// </summary>
        /// <returns></returns>
        public bool ExecutorIsConsole()
        {
            return Executor is IConsole;
        }

        public IPlayer GetExecutorAsPlayer()
        {
            if(ExecutorIsPlayer())
            {
                return (IPlayer)Executor;
            }
            return null;
        }

        public IConsole GetExecutorAsConsole()
        {
            if (ExecutorIsPlayer())
            {
                return (IConsole)Executor;
            }
            return null;
        }

        public PlayerBanEvent(string Reason, IPlayer Victim, IUser Executor)
        {
            this.Reason = Reason;
            this.Victim = Victim;
            this.Executor = Executor;
        }
    }
}
