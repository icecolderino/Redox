using System;
using System.IO;
using System.Collections.Generic;

using Newtonsoft.Json;

using Redox.API.Helpers;
using Redox.Core.Plugins;

namespace Redox.API.Configuration.Translation
{
    public sealed class Translations
    {
        private Plugin _plugin;
        private Dictionary<string, Dictionary<string, string>> _message = new Dictionary<string, Dictionary<string, string>>();

        public bool HasMessages
        {
            get
            {
                return _message.Count > 0;
            }
        }

        public Translations(Plugin plugin)
        {
            _plugin = plugin;
        }

        public void Save()
        {
            string path = Path.Combine(_plugin.FileInfo.DirectoryName, "Translation.json");
            JSONHelper.ToFile(path, this);
            
        }
        
        public void Register(string Language, Dictionary<string, string> messages)
        {
            if(!_message.ContainsKey(Language))
            {
                _message.Add(Language, messages);
            }
        }
        public string Translate(string Language, string Key)
        {           
            if(_message.ContainsKey(Language) && _message[Language].ContainsKey(Key))
            {
                return _message[Language][Key];
            }
            return string.Empty;
        }

        public void LoadTranslation()
        {
            string path = Path.Combine(_plugin.FileInfo.DirectoryName, "Translation.json");

            if(File.Exists(path))
            {
                _message = JSONHelper.FromFile<Dictionary<string, Dictionary<string, string>>>(path);
            }
        }
    }
}
