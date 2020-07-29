using Redox.Core.Commands;

namespace Redox.API.Commands
{
    public class CommandContext : ICommandContext
    {
        public ICommand Command { get; }
        public CommandInfo Info { get; }

        public CommandContext(ICommand command, CommandInfo info)
        {
            this.Command = command;
            this.Info = info;
        }
        
    }
}