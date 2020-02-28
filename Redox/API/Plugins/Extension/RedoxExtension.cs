
namespace Redox.API.Plugins.Extension
{
    public abstract class RedoxExtension
    { 
        public virtual string Title { get; }
        public virtual string Description { get; }
        public virtual string Author { get; }
        public virtual string Version { get; }

        public abstract void Init();

        public virtual void OnExtensionsLoaded() { }
    }
}
