using System;

namespace Redox.API.Plugins.CSharp
{
    /// <summary>
    /// Holds metadata about a plugin.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class PluginInfo : Attribute
    {
        public string Title { get; }
        public string Description { get; }
        public string Authors { get; }
        public string Version { get; }
        public string Url { get; }
        
        public PluginInfo(string title, string description, string authors, string version, string url = "")
        {
            this.Title = title;
            this.Description = description;
            this.Authors = authors;
            this.Version = version;
            this.Url = url;
        }
    }
}