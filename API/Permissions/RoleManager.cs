using Redox.Core.Permissions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Redox.API.Permissions
{
    public sealed class RoleManager : IRoleProvider
    {
        public Task<bool> AddGroupAsync(string role, string group)
        {
            throw new NotImplementedException();
        }

        public Task<bool> CreateRoleAsync(IRole role)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteRoleAsync(string name)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<IGroup>> GetRoleGroupsAsync(string name)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<IRole>> GetRolesAsync()
        {
            throw new NotImplementedException();
        }

        public Task LoadAsync()
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveGroupAsync(string role, string group)
        {
            throw new NotImplementedException();
        }

        public Task SaveAsync()
        {
            throw new NotImplementedException();
        }
    }
}
