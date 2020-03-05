using System;


namespace Redox.API.Plugins.CSharp
{
    [AttributeUsage(AttributeTargets.Method)]
    public class IgnoreCollector : Attribute
    {
        public IgnoreCollector() : base() { }
    }
}
