using System;
using System.Linq;
using System.Collections.Generic;
using Redox.Core.Plugins;

namespace Redox.API.Commands
{
    public class CommandManager
    {
        private static CommandManager instance;
        private readonly Plugin plugin;

        private readonly IList<Command> _commands = new List<Command>();


        public static readonly IList<CommandManager> Commands = new List<CommandManager>();
      

        public static CommandManager GetInstance(Plugin plugin)
        {
            if (instance == null)
                instance = new CommandManager(plugin);
            Commands.Add(instance);
            return instance;
        }

        public CommandManager(Plugin plugin)
        {
            _commands = new List<Command>();
            this.plugin = plugin;          
        }

        public void Register(string command, string description, string[] permissions, CommandFlags commandFlags, Action<CommandExecutor, string[]> action)
        {
            if (_commands.Any(x => x.Name == command))
            {
                _commands.Add(new Command(command, description, permissions, commandFlags, action));
            }
            else
                UnityEngine.Debug.LogWarning(string.Format("[Redox] Plugin {0} tried to register command {1} but its already registered!", plugin.Title, command));
        }

        public void Call(string command, string[] args, CommandExecutor executor)
        {
            if(_commands.Any(x => x.Name == command))
            {
                var action = _commands.First(x => x.Name == command).Action;
                action.DynamicInvoke(executor, args);
            }
        }  
        
        public bool HasCommand(string cmd)
        {
            return _commands.Any(x => x.Name == cmd);
        }
        public Command GetCommand(string cmd)
        {
            if(HasCommand(cmd))
            {
                return _commands.First(x => x.Name == cmd);
            }
            return null;
        }
    }
}
