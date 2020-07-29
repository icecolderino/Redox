using Redox.API.Console;
using Redox.API.Player;
using Redox.Core.User;

namespace Redox.API.Events
{
    public sealed class PlayerKickEvent
    {
        /// <summary>
        /// The reason for the kick
        /// </summary>
        public string Reason { get; }

        /// <summary>
        /// The player that got kicked
        /// </summary>
        public IPlayer Victim { get; }

        /// <summary>
        /// The Executor of the kick command, This either be an console or player
        /// </summary>
        public IUser Executor { get; }

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
            if (ExecutorIsPlayer())
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

        public PlayerKickEvent(string reason, IPlayer victim, IUser executor)
        {
            this.Reason = reason;
            this.Victim = victim;
            this.Executor = executor;
        }
    }
}
