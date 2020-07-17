
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Redox.API.Commands;
using Redox.API.Configuration.Translation;
using Redox.API.Plugins;
using Redox.Core.Commands;

namespace Redox.Core.Plugins
{
    public abstract class Plugin
    {
        public virtual string Title { get { return "Unknown"; } }
        public virtual string Description { get { return "Plugin"; } }
        public virtual string Author { get { return "Unknown"; } }
        public virtual string Credits { get; }
        public virtual Version Version { get { return new Version("1.0.0.0"); } }
        public virtual Version CoreVersion { get { return new Version("0.0.0.0"); } }
        public virtual Uri ResourceURL { get { return new Uri("https://redoxmodding.org"); } }

        public virtual ICommandProvider Commands { get; internal set; }
        public Translations translation { get; set; }
        public virtual PluginCollector Collector { get; }

        public FileInfo FileInfo { get; set; }
        public PluginContainer Container { get; internal set; }
        public abstract Task Initialize();
        public abstract Task Deinitialize();
        protected virtual void LoadDefaultTranslations() { }

        public abstract object Call(string name, params object[] parameters);
        public abstract Task<object> CallAsync(string name, params object[] parameters);

        public abstract void LoadMethods();

        public void CheckTranslation()
        {
            translation = new Translations(this);
            translation.LoadTranslation();
            if (!translation.HasMessages)
                LoadDefaultTranslations();
        }
    }

}
