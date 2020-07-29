using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

using Redox.API;
using Redox.API.Libraries;
using Redox.API.Plugins;

namespace Redox.Core.Plugins
{
    public class PluginEngineManager
    {
        private readonly IList<Type> _engines = new List<Type>();
        private readonly IList<IPluginEngine> _instances = new List<IPluginEngine>();

        private ILogger Logger => Bootstrap.RedoxMod.Logger;

        public void Register<TEngine>() where TEngine : IPluginEngine
        {
            var engine = typeof(TEngine);
            if (!_engines.Contains(engine))
                _engines.Add(engine);
            Logger.LogInfo($"[Redox] Succesfully Registered {engine.Name}");
        }
        public void Unregister(string name)
        {
            var engine = GetEngineByName(name);

            if(engine != null)
            {
                 engine.UnloadPlugins();
                _instances.Remove(engine);
                _engines.Remove(engine.GetType());
               
            }
        }

        public void StartAll()
        {
            foreach (var engine in _engines)
            {
                var instance = (IPluginEngine)Activator.CreateInstance(engine);
                if (!_instances.Contains(instance))
                {
                    Logger.LogInfo($"[Redox] Loading {instance.Language} Engine..");
                    _instances.Add(instance);
                    instance.LoadPlugins();
                    
                }
                else
                   Logger.LogWarning($"[Redox] Skipping engine {engine.Name} because its already loaded!");                   
            }
            Logger.LogInfo($"[Redox] Succesfully loaded {Redox.Mod.Plugins.GetPlugins().Count()} Plugins");
        }
        public void UnloadAll()
        {

            foreach(var instance in _instances)
            {
                Logger.LogInfo($"[Redox] Unloading engine {instance.Language}");
                instance.UnloadPlugins();
            }
        }

        private IPluginEngine GetEngineByName(string name)
        {
            return _instances.SingleOrDefault(x => x.GetType().Name == name);
        }

        internal void LoadPlugin(string dirName)
        {
            string path = Path.Combine(Bootstrap.RedoxMod.PluginPath, dirName);
            if (Directory.Exists(path))
            {
                foreach (var engine in _instances)
                    engine.LoadPlugin(path);
            }
            else
                Logger.LogWarning($"[Redox] There is no plugin folder called {dirName}");
        }

        internal void UnloadPlugin(string name, PluginContainer pc = null)
        {
            foreach (var engine in _instances)
                engine.UnloadPlugin(name, pc);
        }

        internal void ReloadPlugin(string name)
        {
            foreach (var engine in _instances)
                engine.ReloadPlugin(name);
        }

        internal void ReloadAllPlugins()
        {
            foreach (var engine in _instances)
                engine.ReloadPlugins();
        }

        internal void UnloadAllPlugins()
        {
            foreach (var engine in _instances)
                engine.UnloadPlugins();
        }
    }
}
