using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Redox.Core.Permissions
{
    /// <summary>
    /// The role provider is responsible for managing roles.
    /// </summary>
    public interface IRoleProvider
    {
        /// <summary>
        /// Gets all roles.
        /// </summary>
        /// <returns>A list of all roles.</returns>
        Task<IEnumerable<IRole>> GetRolesAsync();

        /// <summary>
        /// Get all groups associated with the role.
        /// </summary>
        /// <param name="name">The name of the role.</param>
        /// <returns></returns>
        Task<IEnumerable<IGroup>> GetRoleGroupsAsync(string name);

        /// <summary>
        /// Creates a new role.
        /// </summary>
        /// <param name="role">The role you want to add</param>
        /// <returns></returns>
        Task<bool> CreateRoleAsync(IRole role);

        /// <summary>
        /// Deletes a role with all its associated groups.
        /// </summary>
        /// <param name="name">The name of the role you want to delete.</param>
        /// <returns></returns>
        Task<bool> DeleteRoleAsync(string name);

        /// <summary>
        /// Adds a new group to the role.
        /// </summary>
        /// <param name="role">The name of the role.</param>
        /// <param name="group">The name of the group.</param>
        /// <returns></returns>
        Task<bool> AddGroupAsync(string role, string group);

        /// <summary>
        /// Removes a group from the role.
        /// </summary>
        /// <param name="role">The name of the role.</param>
        /// <param name="group">The name of the group.</param>
        /// <returns></returns>
        Task<bool> RemoveGroupAsync(string role, string group);

        /// <summary>
        /// Saves the file.
        /// </summary>
        /// <returns></returns>
        Task SaveAsync();

        /// <summary>
        /// Loads the file.
        /// </summary>
        /// <remarks>Doing this will overwrite the current role data.</remarks>
        /// <returns></returns>
        Task LoadAsync();
    }
}
