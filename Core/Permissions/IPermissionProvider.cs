using System.Collections.Generic;
using System.Threading.Tasks;

using Redox.API.Player;

using Redox.Core.Data;


namespace Redox.Core.Permissions
{
    /// <summary>
    /// The permission provider is responsible for managing permissions.
    /// </summary>
    public interface IPermissionProvider : IData
    {
        /// <summary>
        /// Gets the permissions of the player.
        /// </summary>
        /// <param name="player">The Player you want to get permissions from.</param>
        /// <returns></returns>
        Task<IEnumerable<string>> GetPlayerPermissionsAsync(IPlayer player);

        /// <summary>
        /// Grant a permission to the player.
        /// </summary>
        /// <param name="player">The player you want to add the permission to.</param>
        /// <param name="permission">The permission you want to add.</param>
        /// <returns></returns>
        Task GrantPermissionAsync(IPlayer player, string permission);

        /// <summary>
        /// Revoke a permission from the player.
        /// </summary>
        /// <param name="player"></param>
        /// <param name="permission"></param>
        /// <returns></returns>
        Task RevokePermission(IPlayer player, string permission);
    }
}
