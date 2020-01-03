using System.Reflection;
using System.Collections.Generic;

using Redox.Core.Plugins;

namespace Redox.Core.PluginEngines
{
    public interface IPluginEngine
    {
        /// <summary>
        /// List of instances (For Interpreter engines only)
        /// </summary>
        Dictionary<string, object> Values { get; }

        /// <summary>
        /// List of assemblies (For Interpreter engines only)
        /// </summary>
        List<Assembly> Assemblies { get; }

        string Language { get; }

        string Pattern { get; }

        void LoadPlugins();

        void LoadPlugin(string dir);

        void UnloadPlugins();

        void UnloadPlugin(string Name, PluginContainer container = null);

        void ReloadPlugins();
    }
}
