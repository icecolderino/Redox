using Redox.API;
using Redox.API.Commands;
using Redox.API.Configuration;
using Redox.API.Configuration.Translation;
using Redox.API.DependencyInjection;
using Redox.API.Entity;
using Redox.API.Libraries;
using System;

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
        public virtual string Version { get { return "1.0.0.0"; } }
        public virtual string Credits { get; }
        public virtual string ResourceID { get; }

        public virtual string Path { get; internal set; }

        public virtual CommandManager Commands { get; internal set; }
        public virtual Config DefaultConfig { get; internal set; }

        protected virtual Translations DefaultTranslation { get; set; }
        protected virtual PluginCollector Manager { get;  }
        protected virtual IServer Server { get;}
        protected ILogger Logger = DependencyContainer.Resolve<ILogger>();
        protected virtual IEntityManager World { get; }

        public virtual void Initialize() { }
        public virtual void Deinitialize() { }


        protected virtual void LoadDefaultTranslations() { }

        public void CheckTranslation()
        {
            DefaultTranslation = Translations.LoadTranslation(this);

            if (DefaultTranslation == null)
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
