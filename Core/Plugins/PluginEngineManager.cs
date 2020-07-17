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
    public static class PluginEngineManager
    {
        private static readonly IList<Type> _engines = new List<Type>();
        private static readonly IList<IPluginEngine> _instances = new List<IPluginEngine>();

        private static ILogger Logger => Bootstrap.RedoxMod.Logger;

        public static void Register<TEngine>() where TEngine : IPluginEngine
        {
            var engine = typeof(TEngine);
            if (!_engines.Contains(engine))
                _engines.Add(engine);
            Logger.LogInfo($"[Redox] Succesfully Registered {engine.Name}");
        }
        public static void Unregister(string name)
        {
            var engine = GetEngineByName(name);

            if(engine != null)
            {
                 engine.UnloadPlugins();
                _instances.Remove(engine);
                _engines.Remove(engine.GetType());
               
            }
        }

        public static void StartAll()
        {
            foreach (var engine in _engines)
            {
                var instance = (IPluginEngine)Activator.CreateInstance(engine);
                if (!_instances.Contains(instance))
                {
                    Logger.LogInfo(string.Format("[Redox] Loading {0} Engine..", instance.Language));
                    _instances.Add(instance);
                    instance.LoadPlugins();
                    
                }
                else
                   Logger.LogWarning(string.Format("[Redox] Skipping engine {0} because its already loaded!", engine.Name));                   
            }
            Logger.LogInfo($"[Redox] Succesfully loaded {PluginCollector.GetCollector().GetPlugins().Count} Plugins");
        }
        public static void UnloadAll()
        {

            foreach(var instance in _instances)
            {
                Logger.LogInfo(string.Format("[Redox] Unloading engine {0}", instance.Language));
                instance.UnloadPlugins();
            }
        }

        private static IPluginEngine GetEngineByName(string name)
        {
            return _instances.SingleOrDefault(x => x.GetType().Name == name);
        }

        internal static void LoadPlugin(string dirName)
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

        internal static void UnloadPlugin(string name, PluginContainer pc = null)
        {
            foreach (var engine in _instances)
                engine.UnloadPlugin(name, pc);
        }

        internal static void ReloadPlugin(string name)
        {
            foreach (var engine in _instances)
                engine.ReloadPlugin(name);
        }

        internal static void ReloadAllPlugins()
        {
            foreach (var engine in _instances)
                engine.ReloadPlugins();
        }

        internal static void UnloadAllPlugins()
        {
            foreach (var engine in _instances)
                engine.UnloadPlugins();
        }
    }
}
