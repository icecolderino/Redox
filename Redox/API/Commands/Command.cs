using System;

namespace Redox.API.Commands
{
    public sealed class Command
    {
        public readonly string Name;

        public readonly string Description;

        public readonly CommandFlags Flags;

        public readonly Action<CommandExecutor, string[]> Action;

       
        public Command(string Name, string Description, CommandFlags Flags, Action<CommandExecutor, string[]> Action)
        {
            this.Name = Name;
            this.Flags = Flags;
            this.Action = Action;
        }
    }
}
