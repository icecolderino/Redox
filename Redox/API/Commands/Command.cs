using System;

namespace Redox.API.Commands
{
    public sealed class Command
    {
        public string Name { get; }

        public string Description { get; }

        public string[] Permissions { get; }

        public CommandFlags Flags { get; }

        public Action<CommandExecutor, string[]> Action { get; }

       
        public Command(string Name, string Description, string[] Permissions, CommandFlags Flags, Action<CommandExecutor, string[]> Action)
        {
            this.Name = Name;
            this.Description = Description;
            this.Permissions = Permissions;
            this.Flags = Flags;
            this.Action = Action;

        }

    }
}
