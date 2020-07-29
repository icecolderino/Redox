using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Redox.Core.Commands;
using Redox.Core.Plugins;
using RestSharp.Extensions;

namespace Redox.API.Commands
{
    /// <summary>
    /// Command manager
    /// </summary>
    public sealed class CommandManager : ICommandProvider
    {
        private IDictionary<Plugin, IList<ICommandContext>> _commands;

        /// <summary>
        /// Registers a new command.
        /// </summary>
        /// <typeparam name="TCommand">The command class</typeparam>
        /// <param name="plugin">The plugin that is associated with this command.</param>
        /// <returns></returns>
        public Task RegisterAsync<TCommand>(Plugin plugin) where TCommand : ICommand
        {
            if (!_commands.ContainsKey(plugin))
                _commands.Add(plugin, new List<ICommandContext>());
            ICommand command = Activator.CreateInstance<TCommand>();
            CommandInfo commandInfo = command.GetType().GetAttribute<CommandInfo>();
            if ( commandInfo == null)
            {
                Redox.Mod.Logger.LogWarning("[Redox] Failed to register command {0} on {1} because its lacking information.");
                return Task.CompletedTask;
            }
            ICommandContext context = new CommandContext(command, commandInfo);
            _commands[plugin].Add(context);
            return Task.CompletedTask;
        }
        /// <summary>
        /// Unregisters a command.
        /// </summary>
        /// <typeparam name="TCommand">The command class.</typeparam>
        /// <param name="plugin">The plugin that is associated with this command.</param>
        /// <returns></returns>
        public Task UnregisterAsync<TCommand>(Plugin plugin) where TCommand : ICommand
        {
            if (!_commands.ContainsKey(plugin))
                return Task.CompletedTask;

            ICommandContext context = _commands[plugin].FirstOrDefault(x => x.Command.GetType() == typeof(TCommand));
            if(context != null)
                _commands[plugin].Remove(context);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Gets a command by name.
        /// </summary>
        /// <param name="name">The name of the command.</param>
        /// <param name="plugin">The plugin that is associated with this command.</param>
        /// <returns>The context of the command.</returns>
        public Task<ICommandContext> GetCommandAsync(string name, Plugin plugin)
        {
            if (!_commands.ContainsKey(plugin))
                return null;
            ICommandContext command = _commands[plugin].FirstOrDefault(x => x.Info.Name == name);
            if (command == null)
                return null;
            return Task.FromResult(command);
        }
        /// <summary>
        /// Gets if the plugin has the specified command.
        /// </summary>
        /// <param name="name">The name of the command.</param>
        /// <param name="plugin">The plugin that is associated with this command.</param>
        /// <returns></returns>
        public Task<bool> HasCommandAsync(string name, Plugin plugin)
        {
            if (!_commands.ContainsKey(plugin))
                return Task.FromResult(false);
            ICommandContext command = _commands[plugin].FirstOrDefault(x => x.Info.Name == name);
            return Task.FromResult(command != null);
        }

        /// <summary>
        /// Executes a command.
        /// </summary>
        /// <param name="executor">The executor of the command.</param>
        /// <param name="command">The command you want to execute.</param>
        /// <param name="args">The arguments given with this command.</param>
        /// <returns></returns>
        public Task CallAsync(ICommandExecutor executor, string command, string[] args)
        {
            foreach(var list in _commands.Values)
            {
                IEnumerable<ICommandContext> cmds = (from x in list
                                              where x.Info.Name == command
                                              select x);
                foreach (var context in cmds)
                {
                    CommandInfo info = context.Info;
                    ICommand cmd = context.Command;
                    if (executor.IsPlayer)
                    {
                        if (info.Caller == CommandCaller.Player || info.Caller == CommandCaller.Both)
                        {
                            cmd.Execute(executor, args);
                        }
                        else
                            executor.GetPlayer().Message("<color=red>You cannot execute this command!</color>");
                    }
                    else if (executor.IsConsole)
                    {
                        if (info.Caller == CommandCaller.Console|| info.Caller == CommandCaller.Both)
                        {
                            cmd.Execute(executor, args);
                        }
                        else
                            executor.GetConsole().Reply("This command can only be executed by players!");
                    }
                }
            }
            return Task.CompletedTask;
        }


        public CommandManager()
        {
            _commands = new Dictionary<Plugin, IList<ICommandContext>>();
        }
    }
}
