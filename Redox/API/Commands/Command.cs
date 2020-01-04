using System;

namespace Redox.API.Commands
{
    public sealed class Command
    {
        public string Name { get; }

        public string Description { get; }

        public string Permission { get; }

        public CommandFlags Flags { get; }

        public Action<CommandExecutor, string[]> Action { get; }

       
        public Command(string Name, string Description, string Permission, CommandFlags Flags, Action<CommandExecutor, string[]> Action)
        {
            this.Name = Name;
            this.Description = Description;
            this.Permission = Permission;
            this.Flags = Flags;
            this.Action = Action;

        }

    }
}
