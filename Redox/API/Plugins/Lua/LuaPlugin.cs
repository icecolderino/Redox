using System;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using System.Threading.Tasks;

using Redox.Core.Plugins;

using Redox.API.Libraries;

using NLua;
using System.Reflection;
using Redox.API.Commands;

namespace Redox.API.Plugins.Lua
{
    public sealed class LuaPlugin : Plugin
    {
        private readonly IDictionary<string, LuaFunction> Methods = new Dictionary<string, LuaFunction>();
        private readonly IList<Timer> _timers = new List<Timer>();
        private readonly IList<Command> _commands = new List<Command>();
        private readonly string Code;
        private readonly FileInfo Info;
        internal readonly NLua.Lua Engine;


        public LuaPlugin(FileInfo info, string code)
        {
            this.Info = info;
            this.Code = code;

            DefaultConfig = null;

            Engine = new NLua.Lua();
            Engine["os"] = null;
            Engine["io"] = null;
            Engine["dofile"] = null;
            Engine["package"] = null;
            Engine["luanet"] = null;
            Engine["load"] = null;


            Engine["Title"] = Title;
            Engine["Description"] = Description;
            Engine["Author"] = Author;
            Engine["Version"] = Version;
            Engine["Plugin"] = this;
            Engine["Plugins"] = Collector;
            Engine["LocalStorage"] = LocalStorage.GetStorage();
            Engine["SQLite"] = SQLiteConnector.GetInstance();
            Engine["Redox"] = Bootstrap.Mod.GetComponent<Redox>();
            Engine["Util"] = new Util(ref Engine);
            //Register binding flags
            Engine["bf_instance_public"] = BindingFlags.Instance | BindingFlags.Public;
            Engine["bf_instance_private"] = BindingFlags.Instance | BindingFlags.NonPublic;
            Engine["bf_static_public"] = BindingFlags.Static| BindingFlags.Public;
            Engine["bf_static_private"] = BindingFlags.Static | BindingFlags.NonPublic;
            foreach (var pair in Redox.InterpreterVariables)
                Engine[pair.Key] = pair.Value;

        }

        public override object Call(string name, params object[] args)
        {

            try
            {
                if(Methods.ContainsKey(name))
                {
                    return Methods[name].Call(args);
                }
                return null;
            }
            catch(Exception ex)
            {
                Logger.LogError(string.Format("[Lua] Failed to call \"{0}\", Error: {1}", name, ex.Message));
                return null;
            }
        }

        public override T Call<T>(string name, params object[] args)
        {
            throw new NotImplementedException();
        }

        public override Task<object> CallAsync(string name, params object[] args)
        {
            throw new NotImplementedException();
        }

        public override void LoadMethods()
        {
            foreach(string global in Engine.Globals)
            {
                LuaFunction function = Engine[global] as LuaFunction;

                if(function != null)
                {
                    Methods.Add(global, function);
                }

            }
        }

        public Timer CreateTimer(double interval, LuaFunction func)
        {
            Timer timer = Timers.Create(interval, TimerType.Once, () =>
            {
                func.Call();
            });
            _timers.Add(timer);
            return timer;
        }
        public Timer CreateTimer(double interval, LuaFunction func, int rate = 0)
        {
            Timer timer = Timers.Create(interval, TimerType.Repeat, () =>
            {
                func.Call();
            }, rate);
            _timers.Add(timer);
            return timer;
        }

        public Command RegisterCommand(string cmd, string description, string mode, LuaFunction func)
        {
            CommandFlags flag = CommandFlags.Both;

            switch (mode.ToUpper())
            {
                case "BOTH":
                    flag = CommandFlags.Both;
                    break;
                case "PLAYER":
                    flag = CommandFlags.Player;
                    break;
                case "CONSOLE":
                    flag = CommandFlags.Console;
                    break;
            }


            Command command = Commands.Register(cmd, description, string.Empty, flag, (executor, args) =>
            {
                func.Call(executor, args);
                var player = executor.GetPlayer();
            });
            _commands.Add(command);           
            return command;
        }
    }
}
