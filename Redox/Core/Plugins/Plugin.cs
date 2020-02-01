using System;
using System.IO;


using Redox.API;
using Redox.API.Commands;
using Redox.API.Configuration;
using Redox.API.Configuration.Translation;
using Redox.API.DependencyInjection;
using Redox.API.Entity;
using Redox.API.Libraries;

namespace Redox.Core.Plugins
{
    /// <summary>
    /// Generic Base representation of a plugin
    /// </summary>
    public abstract class Plugin : IDisposable
    {

        public virtual string Title { get { return "Unknown"; } }
        public virtual string Description { get { return "Plugin"; } }
        public virtual string Author { get { return "Unknown"; } }
        public virtual string Credits { get; }
        public virtual Version Version { get { return new Version("1.0.0.0"); } }
        public virtual Version CoreVersion { get { return new Version("0.0.0.0"); } }      
        public virtual Uri LicenseURL { get { return new Uri("https://yourlicenseurl.com"); } }     
        public virtual Uri ResourceURL { get { return new Uri("https://redoxmod.org"); } }

    
        public virtual CommandManager Commands { get; internal set; }
        public virtual Config DefaultConfig { get; internal set; }

        protected virtual Translations translation { get; set; }
        protected virtual PluginCollector Collector { get;  }
        protected virtual IServer Server { get;}
        protected ILogger Logger = Redox.Logger;
        protected virtual IEntityManager World { get; }

        public FileInfo FileInfo { get; set; }

        internal virtual void Initialize() { }
        internal virtual void Deinitialize() { }


        protected virtual void LoadDefaultTranslations() { }

        public void CheckTranslation()
        {
            translation = Translations.LoadTranslation(this);

            if (translation == null)
                LoadDefaultTranslations();
        }


        public abstract object Call(string name, params object[] args);
        public abstract T Call<T>(string name, params object[] args);
        public abstract void LoadMethods();


        ~Plugin()
        {
            this.Dispose(false);
        }
        public void Dispose()
        {
            this.Dispose(true);
            System.GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
        }

    }
}
