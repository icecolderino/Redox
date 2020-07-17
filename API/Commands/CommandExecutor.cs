using Redox.API.Console;
using Redox.API.Player;
using Redox.Core.Commands;
using Redox.Core.User;

namespace Redox.API.Commands
{
    public sealed class CommandExecutor : ICommandExecutor
    {
        public IUser User { get; }

        public bool IsPlayer
        {
            get
            {
                return User is IPlayer;
            }
        }

        public bool IsConsole
        {
            get
            {
                return User is IConsole;
            }
        }

        public IConsole GetConsole()
        {
            return User as IConsole;
        }

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
