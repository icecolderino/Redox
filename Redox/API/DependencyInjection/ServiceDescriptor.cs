using System;

namespace Redox.API.DependencyInjection
{
    public class ServiceDescriptor
    {
        public Type Service { get; }

        public Type Implementation { get;}

        public ServiceLifetime Lifetime { get; }

        public object Instance { get; private set; }

        public ServiceDescriptor(Type Service, Type Implementation, ServiceLifetime lifetime)
        {
            this.Service = Service;
            this.Implementation = Implementation;
            this.Lifetime = lifetime;
        }

        public ServiceDescriptor(Type service, ServiceLifetime lifetime)
        {
            this.Service = service;
            this.Lifetime = lifetime;

        }

        public ServiceDescriptor(object implementation, ServiceLifetime lifetime)
        {
            this.Service = implementation.GetType();
            this.Instance = implementation;
            this.Lifetime = lifetime;
        }

        public void CreateInstance()
        {
            var service = Implementation ?? Service;

            if (service == null)
                throw new Exception("Cannot instantiate, implementation is null");

            if (service.IsAbstract || service.IsInterface)
                throw new Exception("Cannot instantiate abstract classes or interfaces");

            var instance = Activator.CreateInstance(service);
            if (Lifetime == ServiceLifetime.Singleton)
                Instance = instance;

            
        }
    }
}
