using System;
using Redox.Core.Commands;

namespace Redox.API.Commands
{
    /// <summary>
    /// Holds metadata about a command.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class CommandInfo : Attribute
    {
        /// <summary>
        /// The name of the command.
        /// <para>Example: info = /info</para>
        /// </summary>
        public string Name { get; }
        /// <summary>
        /// A brief description about this command.
        /// </summary>
        public string Summary { get; }
        
        /// <summary>
        /// The user who can execute this command.
        /// <para>This can be a Player,Console or Both</para>
        /// </summary>
        public CommandCaller Caller { get; }
        
        /// <summary>
        /// Load this command automatically?
        /// </summary>
        public bool AutoLoad { get; }

        public CommandInfo(string name, string summary, CommandCaller caller, bool autoload = true)
        {
            this.Name = name;
            this.Summary = summary;
            this.Caller = caller;
            this.AutoLoad = autoload;
        }
    }
}