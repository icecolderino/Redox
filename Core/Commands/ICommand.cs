using System;
using System.Collections.Generic;
using System.Text;

namespace Redox.Core.Commands
{
    /// <summary>
    /// Represents a in-game command.
    /// <para>
    ///  <b>Commands can be executed by player and the console.</b>
    ///  <b>Commands are defined by a "/" at the start a message.</b>
    /// </para>
    /// </summary>
    public interface ICommand
    {
        /// <summary>
        /// Gets called when someone executes the command.
        /// </summary>
        /// <param name="executor">The executor of the command.</param>
        /// <param name="args">The arguments of the command <para>Example: /info James</para></param>
        void Execute(ICommandExecutor executor, string[] args);
    }
}
