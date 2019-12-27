using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;


using Redox.API.Libraries;
using Redox.Core.Plugins;
using Redox.API.Plugins;
using UnityEngine;

namespace Redox.Core.Plugin
{
    public sealed class PluginLoader
    {
        private static readonly Dictionary<string, Assembly> Plugins = new Dictionary<string, Assembly>();
        private static string path = Redox.PluginPath;

        private const string Expression = "*.dll";

        public static void LoadPlugins()
        {
            foreach (var dir in Directory.GetDirectories(path))
            {
                LoadPlugin(dir);
            }
        }

        public static void LoadPlugin(string dir)
        {
            foreach (var file in Directory.GetFiles(dir, Expression))
            {
                FileInfo info = new FileInfo(file);
                string name = info.Name.Replace(".dll", string.Empty);
                Assembly assembly;

                if (!Plugins.TryGetValue(name, out assembly))
                {
                    assembly = Assembly.Load(File.ReadAllBytes(file));
                    Plugins.Add(name, assembly);
                }

                if (ValidPlugin(assembly))
                {
                    foreach (Type type in assembly.GetExportedTypes())
                    {
                        if (type.IsSubclassOf(typeof(RedoxPlugin)) && type.IsPublic && !type.IsAbstract)
                        {
                            RedoxPlugin plugin = (RedoxPlugin)Activator.CreateInstance(type);
                            PluginContainer container = new PluginContainer(plugin);
                            PluginCollector.GetCollector().AddPlugin(container);

                            container.Plugin.Path = Path.GetDirectoryName(info.Directory.FullName);
                            
                            Logger.LogInfo(string.Format("[Redox] Succesfully loaded plugin {0}, {1}, {2} ({3})", plugin.Title, plugin.Author, plugin.Version, plugin.Description));

                        }
                    }
                }
                else
                    Logger.LogInfo(string.Format("[Redox] Denied plugin {0} because of forbidden references", assembly.FullName));

            }
        }

        private static bool ValidPlugin(Assembly assembly)
        {
            return true;
            /*
            foreach (var dir in Directory.GetDirectories(path))
            {
                foreach (var file in Directory.GetFiles(dir, Expression))
                {
                    return assembly.GetReferencedAssemblies().Any(x => x.Name == new FileInfo(file).Name.Replace(".dll", string.Empty));

                }
            }
            return false;
            */
        }
    }
}