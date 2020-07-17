using Redox.Core.Commands;
using Redox.Core.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Redox.API.Commands
{
    public sealed class CommandManager : ICommandProvider
    {
        private IDictionary<Plugin, IList<ICommand>> commands;

        public Task RegisterAsync<TCommand>(Plugin plugin) where TCommand : ICommand
        {
            if (!commands.ContainsKey(plugin))
                commands.Add(plugin, new List<ICommand>());

            ICommand command = Activator.CreateInstance<TCommand>();
            commands[plugin].Add(command);
            return Task.CompletedTask;
        }

        public Task UnregisterAsync<TCommand>(Plugin plugin) where TCommand : ICommand
        {
            if (!commands.ContainsKey(plugin))
                return Task.CompletedTask;

            ICommand command = commands[plugin].FirstOrDefault(x => x.GetType() == typeof(TCommand));
            if(command != null)
                commands[plugin].Remove(command);
            return Task.CompletedTask;
        }
      

        public Task<ICommand> GetCommandAsync(string name, Plugin plugin)
        {
            if (!commands.ContainsKey(plugin))
                return null;
            ICommand command = commands[plugin].FirstOrDefault(x => x.Name == name);
            if (command == null)
                return null;
            return Task.FromResult(command);
        }

        public Task<bool> HasCommandAsync(string name, Plugin plugin)
        {
            if (!commands.ContainsKey(plugin))
                return Task.FromResult(false);
            ICommand command = commands[plugin].FirstOrDefault(x => x.Name == name);
            return Task.FromResult(command != null);
        }

        public Task CallAsync(ICommandExecutor executor, string command, string[] args)
        {
            foreach(var list in commands.Values)
            {
                IEnumerable<ICommand> cmds = (from x in list
                                              where x.Name == command
                                              select x);
                foreach (var cmd in cmds)
                    cmd.Execute(executor, args);
            }
            return Task.CompletedTask;
        }


        public CommandManager()
        {
            commands = new Dictionary<Plugin, IList<ICommand>>();
        }
    }
}
