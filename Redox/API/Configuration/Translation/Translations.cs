﻿using System;
using System.IO;
using System.Collections.Generic;

using Newtonsoft.Json;
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

            using (FileStream fs = new FileStream(path, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(fs))
                {
                    string json = JsonConvert.SerializeObject(this, Formatting.Indented);
                    writer.Write(json);
                    writer.Dispose();
                    fs.Dispose();
                }
            }

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