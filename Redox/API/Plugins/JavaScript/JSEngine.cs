using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

using Redox.Core.PluginEngines;
using Redox.Core.Plugins;
using Redox.API;
using Redox.API.DependencyInjection;
using Redox.API.Libraries;

namespace Redox.API.Plugins.JavaScript
{
    public class JsEngine : IPluginEngine
    {
        public static JsEngine instance;

        public string Language => "JavaScript";

        public string Pattern => "*.js";

        private readonly string path = Redox.PluginPath;

        private readonly Dictionary<string, JSPlugin> Plugins = new Dictionary<string, JSPlugin>();

        private ILogger Logger => Redox.Logger;

        public void LoadPlugins()
        {

            foreach (string dir in Directory.GetDirectories(path))
                LoadPlugin(dir);
        }


        public void LoadPlugin(string dir)
        {
            foreach (string file in Directory.GetFiles(dir, Pattern))
            {
                FileInfo info = new FileInfo(file);
                string name = Path.GetFileNameWithoutExtension(file);

                if (!Plugins.ContainsKey(name))
                {
                    string code = File.ReadAllText(file);

                    JSPlugin plugin = new JSPlugin(info, name, code);
                    PluginContainer container = new PluginContainer(plugin, null, Language);
                    container.Plugin.PluginPath = info.DirectoryName + "\\";
                    PluginCollector.GetCollector().AddPlugin(container);

                    Logger.LogInfo(string.Format("[JSEngine] Succesfully loaded plugin {0}, Version: {1}, Author: {2} ({3})", plugin.Title, plugin.Version, plugin.Author, plugin.Description));
                }
            }




        }

        public void ReloadPlugins()
        {
            UnloadPlugins();
            LoadPlugins();
        }

        public void UnloadPlugin(string Name, PluginContainer pc = null)
        {
            var container = pc ?? PluginCollector.GetCollector().GetContainer(Name);

            container.Disable();
            PluginCollector.GetCollector().RemovePlugin(container);
            Plugins.Remove(Name);

        }

        public void UnloadPlugins()
        {
            foreach (var container in PluginCollector.GetCollector().GetPlugins().Where(x => x.Language == Language))
                UnloadPlugin(container.Plugin.Title, container);
        }

    }
}