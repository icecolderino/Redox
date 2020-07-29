

using System.Threading.Tasks;
using Redox.Core.Data;

namespace Redox.Core.Configuration
{
    /// <summary>
    /// A configuration provider.
    /// </summary>
    public interface IConfiguration : IData
    {
        /// <summary>
        /// Adds a new setting to the config file
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        Task AddSettingAsync(string key, object value);

        /// <summary>
        /// Changes the value of an existing key
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        Task SetSettingAsync(string key, object value);

        /// <summary>
        /// Gets the value of an existing key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<object> GetSettingAsync(string key);

        /// <summary>
        /// Tries to get the value of an existing key
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        Task<bool> TryGetSettingAsync(string key, out object value);

        /// <summary>
        /// Checks if the configuration has the key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<bool> HasSettingAsync(string key);
    }
}
