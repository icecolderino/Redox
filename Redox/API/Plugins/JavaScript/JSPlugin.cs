using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;


using Jint;
using Jint.Native;
using Jint.Parser;
using Jint.Parser.Ast;
using Redox.API;
using Redox.API.DependencyInjection;
using Redox.API.Libraries;
using Redox.Core.Plugins;

namespace Redox.API.Plugins.JavaScript
{
    public class JSPlugin : Plugin
    {

        public override string Title => engine.GetValue("Title").ToString();
        public override string Description => engine.GetValue("Description").ToString();
        public override string Author => engine.GetValue("Author").ToString();
        public override string Version => engine.GetValue("Version").ToString();
        public override string Credits => engine.GetValue("Credits").ToString();
        public override string ResourceID => engine.GetValue("ResourceID").ToString();

        public readonly string Name;
        public readonly string Code;
        public readonly FileInfo FileInfo;
        public readonly Engine engine;

        public readonly List<FunctionDeclaration> Fuctions = new List<FunctionDeclaration>();

        public JSPlugin(FileInfo info, string name, string code)
        {
            try
            {
                FileInfo = info;
                Name = name;
                Code = code;

                engine = new Engine(cfg => cfg.AllowClr(Redox.InterpreterAssemblies.ToArray()));

                foreach (var pair in Redox.InterpreterVariables)
                    engine.SetValue(pair.Key, pair.Value);
                
                engine.SetValue("Plugin", this);
                engine.SetValue("Datastore", DataStore.GetInstance());
                engine.SetValue("Plugins", PluginCollector.GetCollector());
                engine.SetValue("SQLITE", SQLiteConnector.GetInstance());
                engine.SetValue("Server", this.Server);
                engine.SetValue("World", this.World);
                engine.SetValue("Logger", this.Logger);
                engine.Execute(Code);   
                
            }
            catch (Exception ex)
            {
                Logger.LogError(string.Format("[JSEngine] Failed to load plugin {0}, Error: {1}", Title, ex.Message));
            }

        }

        public override void Initialize()
        {
            Call("Loaded");
        }
        public override void Deinitialize()
        {
            Call("Unloaded");
        }

        public override object Call(string name, params object[] args)
        {
            try
            {
                if (this.Fuctions.Any(x => x.Id.Name == name))
                    return engine.Invoke(name, args);
                return null;
            }
            catch (Exception ex)
            {
                Logger.LogError($"[Redox] Failed to invoke {name} in {Title}, {ex.Message}");
                return null;
            }
        }

        public override T Call<T>(string name, params object[] args)
        {
            return (T)Call(name, args);
        }

        public override void LoadMethods()
        {
            JavaScriptParser parser = new JavaScriptParser();
            foreach (FunctionDeclaration funcDecl in parser.Parse(Code).FunctionDeclarations)
            {
                this.Fuctions.Add(funcDecl);
            }
        }


        public bool HasConfig(string name)
        {
            return File.Exists(Path.Combine(PluginPath, name + ".json"));
        }

        public void CreateConfig(string name, object value)
        {
            JSONHelper.ToFile(Path.Combine(PluginPath, name + ".json"), value);
        }

        public JsValue LoadConfig(string name)
        {
            string path = Path.Combine(PluginPath, name + ".json");

            return JsValue.FromObject(engine, JSONHelper.FromFile<Dictionary<string, object>>(path));
        } 
    }
}