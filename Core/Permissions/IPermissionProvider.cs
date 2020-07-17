using Redox.API.Player;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Redox.Core.Permissions
{
    /// <summary>
    /// The permission provider is responsible for managing permissions.
    /// </summary>
    public interface IPermissionProvider
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

        /// <summary>
        /// Save the file.
        /// </summary>
        /// <returns></returns>
        Task SaveAsync();

        /// <summary>
        /// Load the file.
        /// </summary>
        /// <returns></returns>
        Task LoadAsync();
    }
}
