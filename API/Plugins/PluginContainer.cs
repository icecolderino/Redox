
using Redox.Core.Plugins;
using Redox.API.Commands;
using System.Threading.Tasks;
using Autofac;
using Redox.Core.Commands;

namespace Redox.API.Plugins
{
    public sealed class PluginContainer
    {

        public bool Running
        {
            get;
            private set;
        }    
        
        public Plugin Plugin
        {
            get;
            private set;
        }       
        public string Language
        {
            get;
            private set;
        }

        public PluginContainer(Plugin plugin, object instance, string Language)
        {
            this.Running = true;
            this.Plugin = plugin;
            this.Language = Language;
            this.Plugin.Container = this;
        }
        public async Task Start()
        {
            this.Plugin.LoadMethods();
            this.Plugin.Commands = Redox.Mod.Container.Resolve<ICommandProvider>();
            this.Plugin.CheckTranslation();
            await this.Plugin.Initialize();
            this.Running = true;
        } 

        public async Task Disable()
        {
          //  this.Plugin.Commands.
            await this.Plugin.Deinitialize();
            this.Running = false;
        }    
    }
}
