using Autofac;
using Redox.API.Commands;
using Redox.API.Permissions;
using Redox.Core.Commands;
using Redox.Core.Permissions;
using System;
using System.Collections.Generic;
using System.Text;




namespace Redox
{
    public static class ContainerConfig
    {
        public static event ConfigureContainerDelegate OnConfigureContainer;
        public delegate void ConfigureContainerDelegate(ContainerBuilder builder);
        public static IContainer Configure()
        {
            OnConfigureContainer = delegate (ContainerBuilder cbuilder) { };
            var builder = new ContainerBuilder();
            builder.RegisterType<PermissionsManager>().As<IPermissionProvider>();
            builder.RegisterType<GroupManager>().As<IGroupProvider>();
            builder.RegisterType<RoleManager>().As<IRoleProvider>();
            builder.RegisterType<CommandManager>().As<ICommandProvider>();

            OnConfigureContainer?.Invoke(builder);

            return builder.Build();
        }
    }
}
