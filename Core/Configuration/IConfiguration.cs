

using System.Threading.Tasks;

namespace Redox.Core.Configuration
{
    /// <summary>
    /// A configuration provider.
    /// </summary>
    public interface IConfiguration
    {
        /// <summary>
        /// Adds a new setting to the config file
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        Task AddSetting(string key, object value);

        /// <summary>
        /// Changes the value of an existing key
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        Task SetSetting(string key, object value);

        /// <summary>
        /// Gets the value of an existing key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<object> GetSetting(string key);

        /// <summary>
        /// Tries to get the value of an existing key
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        Task<bool> TryGetSetting(string key, out object value);

        /// <summary>
        /// Checks if the configuration has the key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<bool> HasSetting(string key);

        /// <summary>
        /// Checks if the configuration file exists or not
        /// </summary>
        /// <returns></returns>
        bool Exists { get; }


        Task LoadAsync();

        Task SaveAsync();

    }
}
