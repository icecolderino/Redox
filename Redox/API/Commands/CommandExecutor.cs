using Redox.Core.User;


namespace Redox.API.Commands
{
    public class CommandExecutor
    {
        public IUser User { get; }

        public CommandExecutor(IUser user)
        {
            this.User = user;
        }
    }
}
