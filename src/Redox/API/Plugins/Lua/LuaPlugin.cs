using System;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using System.Threading.Tasks;

using Redox.Core.Plugins;

using Redox.API.Libraries;
using Redox.API.Configuration;

using System.Reflection;
using Redox.API.Commands;
using Redox.API.Collections;

using MoonSharp.Interpreter;
using MoonSharp.Interpreter.Serialization.Json;

namespace Redox.API.Plugins.Lua
{
    public sealed class LuaPlugin : Plugin
    {
        private readonly IDictionary<string, DynValue> Methods = new Dictionary<string, DynValue>();
        private readonly IList<Timer> _timers = new List<Timer>();
        private readonly IList<Command> _commands = new List<Command>();
        private readonly string Code;
        internal readonly Script Engine;
        internal readonly Util util;

        private Config config;

        public override string Title => Engine.Globals["Title"].ToString();
        public override string Description => Engine.Globals["Desciption"].ToString();
        public override string Author => Engine.Globals["Author"].ToString();
        public override Version Version => new Version(Engine.Globals["Version"].ToString());



        internal LuaPlugin(string code)
        {
            this.Code = code;

            Engine = new Script(CoreModules.Json);
            Engine.Globals["os"] = null;
            Engine.Globals["io"] = null;
            Engine.Globals["dofile"] = null;
            Engine.Globals["package"] = null;
            Engine.Globals["luanet"] = null;
            Engine.Globals["load"] = null;

            Engine.Globals["Plugin"] = this;
            Engine.Globals["Plugins"] = Collector;
            Engine.Globals["LocalStorage"] = LocalStorage.GetStorage();
            Engine.Globals["SQLite"] = SQLiteConnector.GetInstance();
            Engine.Globals["Redox"] = Bootstrap.Mod.GetComponent<Redox>();

            Table bf = new Table(Engine);
            bf["public_instance"] = BindingFlags.Instance | BindingFlags.Public;
            bf["private_instance"] = BindingFlags.Instance | BindingFlags.NonPublic;
            bf["public_static"] = BindingFlags.Static | BindingFlags.Public;
            bf["private_static"] = BindingFlags.Static | BindingFlags.NonPublic;
            Engine.Globals["bf"] = bf;

            foreach (var pair in Redox.InterpreterVariables)
                Engine.Globals[pair.Key] = pair.Value;

            Engine.DoString(code);
        }
        

        public override object Call(string name, params object[] args)
        {
            try
            {
                DynValue val = Engine.Globals[name] as DynValue;
                if (val == null) return null;
                Closure func = val.Function;               
                if(func != null)
                {
                    ParseArgs(ref args);
                    return func.Call(args).ToObject();
                }
                return null;
            }
            catch(Exception ex)
            {
                Redox.Logger.LogError($"[Lua] Failed to callhook {name} in {Title} because of error: {ex.Message}");
                return null;
            }
        }

        private void ParseArgs(ref object[] args)
        {
            for(int i = 0; i < args.Length; i++)
            {
                object ob = args[i];
                if (ob is Dictionary<string, object>)
                    args[i] = ConvertDictionaryToTable((Dictionary<string, object>)ob);
                else if (ob is Array)
                    args[i] = TableFromArray((Array)ob);
            }
        }
        private Table TableFromArray(Array array)
        {
            Table table = new Table(Engine);

            for(int i = 0; i < array.Length; i++)
            {
                table[i + 1] = array.GetValue(i);
            }
            return table;
        }

        private Table ConvertDictionaryToTable(Dictionary<string, object> dict)
        {
            Table table = new Table(Engine);

            foreach(KeyValuePair<string, object> pair in dict)
            {
                table[pair.Key] = pair.Value;
            }
            return table;
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
            throw new NotImplementedException();
        }
    }
    /*
    public sealed class LuaPlugin : Plugin
    {
        private readonly IDictionary<string, LuaFunction> Methods = new Dictionary<string, LuaFunction>();
        private readonly IList<Timer> _timers = new List<Timer>();
        private readonly IList<Command> _commands = new List<Command>();
        private readonly string Code;
        private readonly FileInfo Info;
        internal readonly NLua.Lua Engine;
        internal readonly Util util;

        private Config config;

        public override string Title => Engine["Title"].ToString();
        public override string Description => Engine["Desciption"].ToString();
        public override string Author => Engine["Author"].ToString();
        public override Version Version => new Version(Engine["Version"].ToString());
        public LuaPlugin(FileInfo info, string code)
        {
            this.Info = info;
            this.Code = code;
            util = new Util(ref Engine);
            DefaultConfig = null;

            Engine = new NLua.Lua();
        //    Engine.Encoding = System.Text.Encoding.ASCII;
            Engine["os"] = null;
            Engine["io"] = null;
            Engine["dofile"] = null;
            Engine["package"] = null;
            Engine["luanet"] = null;
            Engine["load"] = null;

            Engine["Plugin"] = this;
            Engine["Plugins"] = Collector;
            Engine["LocalStorage"] = LocalStorage.GetStorage();
            Engine["SQLite"] = SQLiteConnector.GetInstance();
            Engine["Redox"] = Bootstrap.Mod.GetComponent<Redox>();
            Engine["Util"] = util;
            //Register binding flags
            Engine["bf_instance_public"] = BindingFlags.Instance | BindingFlags.Public;
            Engine["bf_instance_private"] = BindingFlags.Instance | BindingFlags.NonPublic;
            Engine["bf_static_public"] = BindingFlags.Static | BindingFlags.Public;
            Engine["bf_static_private"] = BindingFlags.Static | BindingFlags.NonPublic;

            foreach (var pair in Redox.InterpreterVariables)
                Engine[pair.Key] = pair.Value;

            Engine.LoadString(Properties.Resources.config, "config").Call();
            Engine.DoString(Code);
        }

        internal override void Initialize()
        {
            Call("Init");
        }
        internal override void Deinitialize()
        {
            Call("DeInit");
        }
        public override object Call(string name, params object[] args)
        {

            try
            {
                LuaFunction func = Engine[name] as LuaFunction;
                if(func != null)
                {
                    for(int i = 0; i < args.Length; i++)
                    {
                        object val = args[i];

                        if (val is Array)
                            args[i] = util.TableFromArray(val as Array);
                    }
                    return func.Call(args);
                }
                return null;
            }
            catch(Exception ex)
            {
                Logger.LogError(string.Format("[Lua] Failed to call \"{0}\", Error: {1}", name, ex));
                return null;
            }
        }

        public override T Call<T>(string name, params object[] args)
        {
            return (T)Call(name, args);
        }

        public override Task<object> CallAsync(string name, params object[] args)
        {
            return null;
        }

        public override void LoadMethods()
        {
            /*
            Redox.Logger.Log("Loading methods..");
            foreach(string global in Engine.Globals)
            {
                LuaFunction function = Engine[global] as LuaFunction;

                if(function != null)
                {
                    Redox.Logger.Log(function.ToString());
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


            Command command = Commands.Register(cmd, description, flag, (executor, args) =>
            {
                func.Call(executor, args);
                var player = executor.GetPlayer();
            });
            _commands.Add(command);           
            return command;
        }

        protected override void LoadDefaultTranslations()
        {
            LuaTable table = Call("LoadDefaultTranslations") as LuaTable;

            if(table != null)
            {
                var dict = util.TableToDictionary(table);
                foreach(var pair in dict)
                {
                    var messages = pair.Value as Dictionary<string, string>;

                    if(messages != null)
                    {
                        translation.Register(pair.Key, messages);
                    }
                }
                translation.Save();
            }
        }

        public bool HasConfig(string name)
        {
            return File.Exists(Path.Combine(FileInfo.DirectoryName, name + ".json"));
        }
        public Config CreateConfig(string name)
        { 
            config = new Config(name, this);
            return config;
        }

        public void SaveConfig(string name)
        {
            if(config != null)
            {
                config.Save();
            }
        }

        public void LoadConfig(string name)
        {
            config = new Config(name, this);
            config.Load();
        }

        public Dictionary<string, object> CreateDictionary()
        {
            return new Dictionary<string, object>();
        }

        public object[] CreateArray()
        {
            return Array.Empty<object>();
        }
    }
    */
}
