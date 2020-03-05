using Redox.API.Console;
using Redox.API.Player;
using Redox.Core.User;


namespace Redox.API.Commands
{
    public class CommandExecutor
    {
        public readonly IUser User;

        public CommandExecutor(IUser user)
        {
            this.User = user;
        }

        public IConsole GetConsole()
        {
            return User as IConsole;
        }
        public IPlayer GetPlayer()
        {
            return User as IPlayer;
        }
        public bool IsPlayer()
        {
            return User is IPlayer;
        }
        public bool IsConsole()
        {
            return User is IConsole;
        }
    }
}
