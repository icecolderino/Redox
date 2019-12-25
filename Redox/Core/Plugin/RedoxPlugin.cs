
using Redox.API.Commands;

namespace Redox.Core.Plugin
{
    public abstract class RedoxPlugin : System.IDisposable
    {

        protected CommandManager Commands { get; }
       

        public virtual string Title { get { return "Unknown"; } }
        public virtual string Description { get { return "Plugin"; } }
        public virtual string Author { get { return "Unknown"; } }
        public virtual string Version { get { return "1.0.0.0"; } }
        public virtual string Credits { get;}
        public virtual string ResourceID { get; }
        

        public string Path { get; }

        public abstract void Start();

        public abstract void Disable();

        public RedoxPlugin()
        {
            Commands = CommandManager.GetInstance(this);
        }

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
            