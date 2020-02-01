using System;


namespace Redox.API.Plugins
{
    [AttributeUsage(AttributeTargets.Method)]
    public class IgnoreCollector : Attribute
    {
        public IgnoreCollector() : base() { }
    }
}
