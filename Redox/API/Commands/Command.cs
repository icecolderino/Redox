using System;

namespace Redox.API.Commands
{
    internal sealed class Command
    {
        public string Name { get; }

        public string Description { get; }

        public CommandFlags Flags { get; }

        public Action<CommandExecutor, string[]> Action { get; }

       
        public Command(string Name, string Description, CommandFlags Flags, Action<CommandExecutor, string[]> Action)
        {
            this.Name = Name;
            this.Description = Description;
            this.Flags = Flags;
            this.Action = Action;

        }

    }
}
