using System.Reflection;
using System.Collections.Generic;

using Redox.API.Plugins;

namespace Redox.Core.Plugins
{
    public interface IPluginEngine
    {
        string Language { get; }

        string Pattern { get; }

        void LoadPlugins();

        void LoadPlugin(string dir);

        void UnloadPlugins();

        void UnloadPlugin(string name, PluginContainer container = null);

        void ReloadPlugins();

        void ReloadPlugin(string name);
    }
}
