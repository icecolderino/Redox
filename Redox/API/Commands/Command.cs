using System;

namespace Redox.API.Commands
{
    public sealed class Command
    {
        public readonly string Name;

        public readonly string Description;

        public readonly CommandCaller Flags;

        public readonly Action<CommandExecutor, string[]> Action;

       
        public Command(string Name, string Description, CommandCaller Flags, Action<CommandExecutor, string[]> Action)
        {
            this.Name = Name;
            this.Flags = Flags;
            this.Action = Action;
        }
    }
}
