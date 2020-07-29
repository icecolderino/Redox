using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Redox.Core.Data;

namespace Redox.Core.Permissions
{
    /// <summary>
    /// The role provider is responsible for managing roles.
    /// </summary>
    public interface IRoleProvider : IData
    {
        /// <summary>
        /// Gets all groups.
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<IRole>> GetRolesAsync();

        /// <summary>
        /// Returns a group with the given name.
        /// </summary>
        /// <param name="name">The name of the group.</param>
        /// <returns></returns>
        Task<IRole> GetRoleAsync(string name);


        /// <summary>
        /// Create a new group.
        /// </summary>
        /// <param name="group">The group you want to add.</param>
        /// <returns></returns>
        Task CreateRoleAsync(IRole group);

        /// <summary>
        /// Removes a group with the given name.
        /// </summary>
        /// <param name="name">The name of the group.</param>
        /// <returns></returns>
        Task RemoveRoleAsync(string name);

        /// <summary>
        /// Gets all groups associated with the player.
        /// </summary>
        /// <param name="id">The id of the player.</param>
        /// <returns></returns>
        Task<IEnumerable<IRole>> GetPlayerRolesAsync(ulong id);

        /// <summary>
        /// Adds a player to a group.
        /// </summary>
        /// <param name="id">The id of the player.</param>
        /// <param name="name">The name of the group.</param>
        /// <returns></returns>
        Task AddPlayerAsync(ulong id, string name);

        /// <summary>
        /// Removes a player to a group.
        /// </summary>
        /// <param name="id">The id of the player.</param>
        /// <param name="name">The name of the group.</param>
        /// <returns></returns>
        Task RemovePlayerAsync(ulong id, string name);

        /// <summary>
        /// Adds a permission to a group.
        /// </summary>
        /// <param name="permission"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        Task AddPermissionAsync(string permission, string name);

        // <summary>
        /// Removes a permission to a group.
        /// </summary>
        /// <param name="permission"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        Task RemovePermissionAsync(string permission, string name);
    }
}
    
