

using Autofac;
using Redox.API;
using Redox.API.Commands;
using Redox.API.Libraries;
using Redox.API.Permissions;
using Redox.API.Plugins;
using Redox.API.Plugins.CSharp;
using Redox.Core.Commands;
using Redox.Core.Permissions;
using Redox.Core.Plugins;

namespace Redox
{
    internal static class ContainerConfig
    {
        internal static void Configure()
        {
            var builder = Redox.Mod.Builder = new ContainerBuilder();
            builder.RegisterType<PermissionsManager>().As<IPermissionProvider>();
            builder.RegisterType<RoleManager>().As<IRoleProvider>();
            builder.RegisterType<CommandManager>().As<ICommandProvider>();

            builder.RegisterInstance(new PluginEngineManager());
            builder.RegisterInstance(new PluginManager());
            builder.RegisterInstance(new DataStore());
            builder.RegisterInstance(new Timers());
            builder.RegisterInstance(new Web());
        }

        internal static void ResolveAll()
        {
            //Lets resolve all dependencies
            var mod = Redox.Mod;
            mod.Logger = mod.Container.Resolve<ILogger>();
            mod.RoleManager = mod.Container.Resolve<IRoleProvider>();
            mod.PermissionManager =  mod.Container.Resolve<IPermissionProvider>();
            mod.EngineManager =  mod.Container.Resolve<PluginEngineManager>();
            mod.EngineManager.Register<CSPluginEngine>();
            mod.Plugins =  mod.Container.Resolve<PluginManager>();
            mod.Timers =  mod.Container.Resolve<Timers>();
        }
    }
}
