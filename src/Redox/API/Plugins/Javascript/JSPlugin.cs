using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using Redox.Core.Plugins;

using Redox.API.Helpers;
using Redox.API.Commands;
using Redox.API.Libraries;
using Redox.API.DependencyInjection;

using Jint;
using Jint.Parser;
using Jint.Runtime.Interop;
using Jint.Native;
using Jint.Parser.Ast;
using UnityEngine;
using Redox.API.Data;

namespace Redox.API.Plugins.Javascript
{
    public sealed class JSPlugin : Plugin
    {
        public override string Title => Engine.GetValue("Title").ToString();
        public override string Description => Engine.GetValue("Description").ToString();
        public override string Author => Engine.GetValue("Author").ToString();
        public override Version Version => new Version(Engine.GetValue("Version").ToString());

        private readonly Engine Engine;
        private readonly string Code;

        private readonly Dictionary<string, JsValue> Functions = new Dictionary<string, JsValue>();
        private readonly Dictionary<string, Command> RegisteredCommands = new Dictionary<string, Command>();

        public JSPlugin(string code)
        {
            this.Code = code;
            Engine = new Engine(cfg => cfg.AllowClr(Redox.InterpreterAssemblies.ToArray()));         
            Engine.SetValue("Plugin", this);
            Engine.SetValue("Server", DependencyContainer.Resolve<IServer>());
            Engine.SetValue("Logger", Redox.Logger);
            Engine.SetValue("Plugins", PluginCollector.GetCollector());
            //    Engine.SetValue("Web", TypeReference.CreateTypeReference(Engine, typeof(Web)));
            Engine.SetValue("Storage", TypeReference.CreateTypeReference(Engine, typeof(LocalStorage)));
            Engine.SetValue("Util", TypeReference.CreateTypeReference(Engine, typeof(Util)));
            Engine.SetValue("Json", TypeReference.CreateTypeReference(Engine, typeof(JSONHelper)));
          //  Engine.SetValue("SQLite", TypeReference.CreateTypeReference(Engine, typeof(SQLite)));
            foreach (var pair in Redox.InterpreterVariables) 
            {
                Engine.SetValue(pair.Key, pair.Value);
            }       
            Engine.Execute(code);
        }

        public void RegisterCommand(string name, string description, string flag, JsValue func)
        {
            CommandCaller cflag;

            switch(flag.ToUpper())
            {
                case "BOTH":
                    cflag = CommandCaller.Both;
                    break;
                case "CONSOLE":
                    cflag = CommandCaller.Console;
                    break;
                case "PLAYER":
                    cflag = CommandCaller.Player;
                    break;
                default:
                    cflag = CommandCaller.Both;
                    break;
            }
            Command command = Commands.Register(name, description, cflag, (executor, args) =>
            {
                CallFunction(func, executor, args);
            });
            RegisteredCommands.Add(name, command);
        }


        public void GET(string url, JsValue func, string[] headers = null)
        {
            Web.GET(url, (code, response) =>
            {
                CallFunction(func, code, response);
            }, headers);
        }

        public Datafile CreateDatafile(string name)
        {
            return new Datafile(name);
        }
        public BinaryDatafile CreateBinaryDatafile(string name)
        {
            return new BinaryDatafile(name);
        }
        private void CallFunction(JsValue func, params object[] args)
        {
            Engine.Invoke(func, args);
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
                if(Functions.ContainsKey(name))
                {
                    return Engine.Invoke(Functions[name], args).ToObject();
                }
                return null;
            }
            catch(Exception ex)
            {
                Logger.LogError($"[Redox] Failed to callhook \"{name}\" in {Title} because of error: {ex.Message}");
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
            JavaScriptParser parser = new JavaScriptParser();
            foreach (FunctionDeclaration func in parser.Parse(Code).FunctionDeclarations)
                Functions.Add(func.Id.Name, Engine.Global.Get(func.Id.Name));
        }
    }
}
