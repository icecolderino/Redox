using System;
using System.IO;
using System.Collections.Generic;
using System.Globalization;

using Redox.Core.Plugins;


namespace Redox.API.Configuration.Translation
{
    public sealed class Translations
    {
        private Plugin _plugin;
        private Dictionary<string, Dictionary<string, string>> messages = new Dictionary<string, Dictionary<string, string>>();

        public bool HasMessages
        {
            get
            {
                return messages.Count > 0;
            }
        }

        public Translations(Plugin plugin)
        {
            _plugin = plugin;
        }

        public void Save()
        {
            string path = Path.Combine(_plugin.FileInfo.DirectoryName, "Translation.json");
            Utility.Json.ToFile(path, messages);
            
        }
        
        public void Register(string Language, Dictionary<string, string> msg)
        {
            if(!messages.ContainsKey(Language))
            {
                messages.Add(Language, msg);
            }
        }

        public string Translate(CultureInfo culture, string key)
        {
            return Translate(culture.DisplayName.ToLower(), key);
        }

        public string Translate(string Language, string Key)
        {           
            if(messages.ContainsKey(Language) && messages[Language].ContainsKey(Key))
            {
                return messages[Language][Key];
            }
            return string.Empty;
        }

        public void LoadTranslation()
        {
            string path = Path.Combine(_plugin.FileInfo.DirectoryName, "Translation.json");

            if(File.Exists(path))
            {
                messages = Utility.Json.FromFile<Dictionary<string, Dictionary<string, string>>>(path);
            }
        }
    }
}
