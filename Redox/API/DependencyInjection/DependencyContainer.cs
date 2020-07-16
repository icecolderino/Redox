using System;
using System.Linq;
using System.Collections.Generic;


namespace Redox.API.DependencyInjection
{
    public static class DependencyContainer
    {
        private static readonly IList<ServiceDescriptor> _descriptors = new List<ServiceDescriptor>();

        public static void RegisterSingleton<TService, TImplementation>() where TImplementation : TService
        {
            _descriptors.Add(new ServiceDescriptor(typeof(TService), typeof(TImplementation), ServiceLifetime.Singleton));
        }

        public static void RegisterSingleton<TService>()
        {
            _descriptors.Add(new ServiceDescriptor(typeof(TService), ServiceLifetime.Singleton));
        }
        public static void RegisterSingleton<TService>(TService implementation)
        {
            _descriptors.Add(new ServiceDescriptor(implementation, ServiceLifetime.Singleton));
        }


        public static void RegisterTransient<TService, TImplementation>() where TImplementation : TService
        {
            _descriptors.Add(new ServiceDescriptor(typeof(TService), typeof(TImplementation), ServiceLifetime.Transient));
        }

        public static void RegisterTransient<TService>()
        {
            _descriptors.Add(new ServiceDescriptor(typeof(TService), ServiceLifetime.Transient));
        }

        public static void RegisterTransient<TService>(TService implementation)
        {
            _descriptors.Add(new ServiceDescriptor(implementation, ServiceLifetime.Transient));
        }

        public static T Resolve<T>()
        {
            return (T)Resolve(typeof(T));
        }
        public static object Resolve(Type Service)
        {        
            var descriptor = _descriptors.SingleOrDefault(x => x.Service == Service);

            if (descriptor == null)
                throw new Exception(string.Format("Service of type {0} isn't registered", Service.Name));

            if (descriptor.Instance != null)
                return descriptor.Instance;

            descriptor.CreateInstance();

            return descriptor.Instance;
        }
    }
}
