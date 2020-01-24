﻿using System.Reflection;
using System.Collections.Generic;

using Redox.Core.Plugins;

namespace Redox.Core.PluginEngines
{
    public interface IPluginEngine
    {
        string Language { get; }

        string Pattern { get; }

        void LoadPlugins();

        void LoadPlugin(string dir);

        void UnloadPlugins();

        void UnloadPlugin(string Name, PluginContainer container = null);

        void ReloadPlugins();
    }
}
