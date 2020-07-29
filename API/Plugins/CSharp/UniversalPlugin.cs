
using Autofac;
using Redox.API.Permissions;
using Redox.Core.Permissions;
using Redox.Core.Plugins;

namespace Redox.API.Plugins.CSharp
{
    public abstract class UniversalPlugin : CSPlugin
    {
        protected IServer Server = Redox.Mod.Container.Resolve<IServer>();
        protected ILogger Logger = Redox.Mod.Logger;     
    }
}
            