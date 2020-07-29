namespace Redox.Core.Plugins
{
    /// <summary>
    /// Base representation of a plugin.
    /// </summary>
    public interface IPlugin
    {
        string Title { get; }
        string Description { get; }
        string Authors { get; }
        string Version { get; }
        
        
    }
}