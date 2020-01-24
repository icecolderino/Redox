using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

using Redox.API;
using Redox.API.Libraries;

namespace Redox.Core.PluginEngines
{
    public static class PluginEngines
    {
        private static readonly IList<Type> _engines = new List<Type>();
        private static readonly IList<IPluginEngine> _instances = new List<IPluginEngine>();

        private static ILogger Logger => Redox.Logger;

        public static void Register<TEngine>() where TEngine : IPluginEngine
        {
            var engine = typeof(TEngine);
            if (!_engines.Contains(engine))
                _engines.Add(engine);
            Logger.LogColor($"[Redox] Succesfully Registered {engine.Name}", ConsoleColor.Cyan);
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
            var logger = Redox.Logger;
            foreach (var engine in _engines)
            {
                var instance = (IPluginEngine)Activator.CreateInstance(engine);
                if (!_instances.Contains(instance))
                {
                    logger.LogColor(string.Format("[Redox] Loading {0} Engine..", instance.Language), ConsoleColor.Cyan);
                    _instances.Add(instance);
                    instance.LoadPlugins();
                    
                }
                else
                   logger.LogWarning(string.Format("[Redox] Skipping engine {0} because its already loaded!", engine.Name));                   
            }
            logger.LogInfo($"[Redox] Succesfully loaded {PluginCollector.GetCollector().GetPlugins().Count} Plugins");
            PluginCollector.GetCollector().CallHook("OnPluginsLoaded");
        }
        public static void UnloadAll()
        {

            foreach(var instance in _instances)
            {
                Logger.LogColor(string.Format("[Redox] Unloading engine {0}", instance.Language), ConsoleColor.DarkBlue);
                instance.UnloadPlugins();
            }
        }

        private static IPluginEngine GetEngineByName(string name)
        {
            return _instances.SingleOrDefault(x => x.GetType().Name == name);
        }
    }
}
