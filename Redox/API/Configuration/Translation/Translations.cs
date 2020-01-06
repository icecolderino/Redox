using System;
using System.IO;
using System.Collections.Generic;

using Newtonsoft.Json;

using Redox.API.Libraries;
using Redox.Core.Plugins;

namespace Redox.API.Configuration.Translation
{
    public sealed class Translations : Dictionary<string, Translation>
    {
        private Plugin _plugin;

        public Translations() : base() { }

        public void Save(Plugin plugin)
        {
            _plugin = plugin;
            string path = Path.Combine(plugin.Path, "Translation.json");
            JSONHelper.ToFile(path, this);
            
        }
        
        public string Translate(string Language, string Key)
        {           
            return base[Language]?[Key] ?? string.Empty;
        }

        public static Translations LoadTranslation (Plugin plugin)
        {
            string path = Path.Combine(plugin.Path, "Translation.json");

            if(File.Exists(path))
            {
                Translations translation = JsonConvert.DeserializeObject<Translations>(File.ReadAllText(path));
                return translation;
            }
            return null;
        }
    }
}
