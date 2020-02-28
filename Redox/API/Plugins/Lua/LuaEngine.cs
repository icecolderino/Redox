using System;
using System.IO;
using System.Collections.Generic;

using Redox.Core.PluginEngines;

namespace Redox.API.Plugins.Lua
{
    public sealed class LuaEngine : IPluginEngine
    {
        public string Language => "Lua";

        public string Pattern => "*.lua";

        public void LoadPlugin(string dir)
        {
            throw new NotImplementedException();
        }

        public void LoadPlugins()
        {
            throw new NotImplementedException();
        }

        public void ReloadPlugin(string Name)
        {
            throw new NotImplementedException();
        }

        public void ReloadPlugins()
        {
            throw new NotImplementedException();
        }

        public void UnloadPlugin(string Name, PluginContainer container = null)
        {
            throw new NotImplementedException();
        }

        public void UnloadPlugins()
        {
            throw new NotImplementedException();
        }
    }
}
