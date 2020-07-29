using Newtonsoft.Json;

namespace Redox.API.Configuration
{
    public class RedoxConfig
    {
        [JsonProperty("This message will be sent when an executed command doesn't exist.")]
        public string UnknownCommand { get; private set; }
        [JsonProperty("Plugin security prevents plugins from having resources.")]
        public bool PluginSecurity { get; private set; }
        [JsonProperty("Do you want outdated plugins to be loaded? (It's recommended to keep this disabled)")]
        public bool LoadIncompitablePlugins { get; private set; }

        [JsonProperty("Do you want Redox to log messages into the console?")]
        public bool Logging { get; private set; }

        [JsonProperty("Do you want to debug messages to show in the console? (This contains the load time of plugins)")]
        public bool DebugLogging { get; private set; }
        [JsonProperty("Enter here the plugin names you want to bypass the security check (Only works when PluginSecurity is enabled).")]
        public string[] WhitelistedAssemblyNames { get; private set; }

        public RedoxConfig Init()
        {
            UnknownCommand = "Unknown Command!";
            PluginSecurity = true;
            LoadIncompitablePlugins = false;
            Logging = true;
            DebugLogging = true;
            WhitelistedAssemblyNames = new string[] { };
            return this;
        }
    }
}