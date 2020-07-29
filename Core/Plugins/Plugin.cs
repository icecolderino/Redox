
using System;
using System.IO;
using System.Threading.Tasks;
using Autofac;
using Redox.API.Libraries;
using Redox.API.Localization;
using Redox.API.Plugins;
using Redox.API.Plugins.CSharp;
using Redox.Core.Commands;
using Redox.Core.Permissions;
using RestSharp.Extensions;

namespace Redox.Core.Plugins
{
    public abstract class Plugin
    {
        /*
        public virtual string Title { get { return "Unknown"; } }
        public virtual string Description { get { return "Plugin"; } }
        public virtual string Author { get { return "Unknown"; } }
        public virtual Version Version { get { return new Version("1.0.0.0"); } }
        public virtual Version CoreVersion { get { return new Version("0.0.0.0"); } }
        public virtual Uri ResourceUrl { get { return new Uri("https://redoxmodding.org"); } }
        */
        
        public PluginInfo Info { get; }
        public ICommandProvider Commands { get; set; }
        
        public PluginManager Plugins { get; }
        public TranslationList Translator { get; private set; }
        
        public FileInfo FileInfo { get; set; }

        public bool Running { get; internal set; }

        //public PluginContainer Container { get; internal set; }
        
        protected readonly Timers Timers = Redox.Mod.Container.Resolve<Timers>();
        protected readonly IPermissionProvider Permissions = Redox.Mod.PermissionManager;
        protected readonly IRoleProvider RoleManager = Redox.Mod.RoleManager;
        public abstract void Initialize();
        public abstract void Deinitialize();
        protected virtual void LoadDefaultTranslations() { }

        public abstract object Call(string name, params object[] parameters);
        public abstract Task<object> CallAsync(string name, params object[] parameters);

        public abstract void LoadMethods();

        public async Task CheckTranslation()
        {
            Translator = new TranslationList(this);
            await Translator.LoadAsync();
            if(Translator.IsEmpty)
                LoadDefaultTranslations();
        }

        public Plugin()
        {
            this.Info = GetType().GetAttribute<PluginInfo>();
            this.Plugins = Redox.Mod.Plugins;
        }
    }

}
