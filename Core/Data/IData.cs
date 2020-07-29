using System.Threading.Tasks;

namespace Redox.Core.Data
{
    /// <summary>
    /// Represents an implementation that can be saved and loaded.
    /// </summary>
    public interface IData
    {
        /// <summary>
        /// Checks if the configuration file exists or not
        /// </summary>
        /// <returns></returns>
        bool Exists { get; }
        
        /// <summary>
        /// Saves the file.
        /// </summary>
        /// <returns></returns>
        Task SaveAsync();
        
        /// <summary>
        /// Loads the file.
        /// </summary>
        /// <returns></returns>
        Task LoadAsync();
    }
}