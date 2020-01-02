

using System.Threading.Tasks;

namespace Redox.Core.Configuration
{
    /// <summary>
    /// Base interface for an configuration
    /// </summary>
    public interface IConfiguration
    {
        /// <summary>
        /// Adds a new setting to the config file
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        void AddSetting(string key, object value);

        /// <summary>
        /// Changes the value of an existing key
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        void SetSetting(string key, object value);

        /// <summary>
        /// Gets the value of an existing key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        object GetSetting(string key);

        /// <summary>
        /// Tries to get the value of an existing key
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        bool TryGetSetting(string key, out object value);

        /// <summary>
        /// Checks if the configuration has the key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        bool HasSetting(string key);

        /// <summary>
        /// Checks if the configuration file exists or not
        /// </summary>
        /// <returns></returns>
        bool Exists();

        /// <summary>
        /// Loads the existing configuration file
        /// </summary>
        Task LoadConfig();

        /// <summary>
        /// Saves the configuration file
        /// </summary>
        Task Save();

    }
}
