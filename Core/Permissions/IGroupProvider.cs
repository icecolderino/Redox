using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Redox.Core.Permissions
{
    /// <summary>
    /// The group provider is responsible for managing groups.
    /// </summary>
    public interface IGroupProvider
    {
        /// <summary>
        /// Gets all groups.
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<IGroup>> GetGroupsAsync();

        /// <summary>
        /// Returns a group with the given name.
        /// </summary>
        /// <param name="name">The name of the group.</param>
        /// <returns></returns>
        Task<IGroup> GetGroupAsync(string name);


        /// <summary>
        /// Create a new group.
        /// </summary>
        /// <param name="group">The group you want to add.</param>
        /// <returns></returns>
        Task CreateGroupAsync(IGroup group);

        /// <summary>
        /// Removes a group with the given name.
        /// </summary>
        /// <param name="name">The name of the group.</param>
        /// <returns></returns>
        Task RemoveGroupAsync(string name);

        /// <summary>
        /// Gets all groups associated with the player.
        /// </summary>
        /// <param name="id">The id of the player.</param>
        /// <returns></returns>
        Task<IEnumerable<IGroup>> GetPlayerGroupsAsync(ulong id);

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
