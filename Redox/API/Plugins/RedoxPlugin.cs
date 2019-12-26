
using Redox.API.Commands;
using Redox.API.Configuration;
using Redox.API.Libraries;

namespace Redox.API.Plugins
{
    public abstract class RedoxPlugin : System.IDisposable
    {

        protected PluginCollector Manager = PluginCollector.GetCollector();

        public CommandManager Commands { get; internal set; }

        public Config DefaultConfig { get; internal set; }

        public virtual string Title { get { return "Unknown"; } }
        public virtual string Description { get { return "Plugin"; } }
        public virtual string Author { get { return "Unknown"; } }
        public virtual string Version { get { return "1.0.0.0"; } }
        public virtual string Credits { get;}
        public virtual string ResourceID { get; }
        

        public string Path { get; internal set; }



        public abstract void Load();

        public abstract void Unload();


        ~RedoxPlugin()
        {
            this.Dispose(false);
        }
        public void Dispose()
        {
            this.Dispose(true);
            System.GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
        }

    }
}
            