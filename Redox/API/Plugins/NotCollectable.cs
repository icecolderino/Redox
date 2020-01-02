using System;


namespace Redox.API.Plugins
{
    [AttributeUsage(AttributeTargets.Method)]
    public class NotCollectable : Attribute
    {
        public NotCollectable() : base() { }
    }
}
