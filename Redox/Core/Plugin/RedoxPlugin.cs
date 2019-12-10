
namespace Redox.Core.Plugin
{
    public abstract class RedoxPlugin : System.IDisposable
    {



        public virtual string Title { get;  }
        public virtual string Description { get;}
        public virtual string Author { get;}
        public virtual string Credits { get;}
        public virtual string ResourceID { get; }
        public virtual string Version { get;}

        public string Path { get; }

        public abstract void Start();

        public abstract void Disable();



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
