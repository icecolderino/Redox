using Redox.API.Commands;

namespace Redox.Core.Commands
{
    public interface ICommandContext
    {
        /// <summary>
        /// The command associated with this context.
        /// </summary>
        ICommand Command { get; }
        /// <summary>
        /// The information about this command.
        /// </summary>
        CommandInfo Info { get; }
    }
}