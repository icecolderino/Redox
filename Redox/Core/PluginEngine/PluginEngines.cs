using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

using Redox.API;
using Redox.API.DependencyInjection;


namespace Redox.Core.PluginEngines
{
    public static class PluginEngines
    {
        private static readonly IList<Type> _engines = new List<Type>();
        private static readonly IList<IPluginEngine> _instances = new List<IPluginEngine>();

        public static void Register<TEngine>() where TEngine : IPluginEngine
        {
            var engine = typeof(TEngine);
            if (!_engines.Contains(engine))
                _engines.Add(engine);
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

        public static void AddAssembly(Assembly assembly)
        {
            foreach (var engine in _instances)
                engine.Assemblies.Add(assembly);

        }

        public static void AddInstance(string name, object instance)
        {
            foreach (var engine in _instances)
                engine.Values.Add(name, instance);

        }

        public static void StartAll()
        {
            var logger = DependencyContainer.Resolve<ILogger>();
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
        }
        private static IPluginEngine GetEngineByName(string name)
        {
            return _instances.SingleOrDefault(x => x.GetType().Name == name);
        }
    }
}
