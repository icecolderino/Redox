using Redox.Core.Permissions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Redox.API.Permissions
{
    public sealed class RoleManager : IRoleProvider
    {
        private readonly string _filepath = Path.Combine(Redox.Mod.DataPath, "redox.roles.json");

        private IList<IRole> _roles;
        
        public bool Exists => File.Exists(_filepath);

        public Task<IEnumerable<IRole>> GetRolesAsync()
        {
            return Task.FromResult(_roles.AsEnumerable());
        }

        public Task<IRole> GetRoleAsync(string name)
        {
            IRole role = _roles.FirstOrDefault(x => x.Name == name);
            return Task.FromResult(role);
        }

        public Task CreateRoleAsync(IRole role)
        {
            if (_roles.Any(x => x.Name == role.Name))
                return Task.CompletedTask;
            _roles.Add(role);
            return Task.CompletedTask;
        }

        public Task RemoveRoleAsync(string name)
        {
            if (_roles.Any(x => x.Name == name))
                _roles.Remove(_roles.FirstOrDefault(x => x.Name == name));
            return Task.CompletedTask;
        }

        public Task<IEnumerable<IRole>> GetPlayerRolesAsync(ulong id)
        {
            return Task.FromResult(_roles.AsEnumerable());
        }

        public async Task AddPlayerAsync(ulong id, string name)
        {
            IRole role = await GetRoleAsync(name);
            if (role != null)
            {
                if (!role.Members.Contains(id))
                    role.Members.Add(id);
            }
        }

        public async Task RemovePlayerAsync(ulong id, string name)
        {
            IRole role = await GetRoleAsync(name);
            if (role != null)
            {
                if (role.Members.Contains(id))
                    role.Members.Remove(id);
            }
        }

        public async Task AddPermissionAsync(string permission, string name)
        {
            IRole role = await GetRoleAsync(name);
            if (role != null)
            {
                if (!role.Permissions.Contains(permission))
                    role.Permissions.Add(permission);
            }
        }

        public async Task RemovePermissionAsync(string permission, string name)
        {
            IRole role = await GetRoleAsync(name);
            if (role != null)
            {
                if (role.Permissions.Contains(permission))
                    role.Permissions.Remove(permission);
            }
        }
        
        public async Task SaveAsync()
        {
            await Utility.Json.ToFileAsync(_filepath, _roles);
        }

        public async Task LoadAsync()
        {
            _roles = await Utility.Json.FromFileAsync<List<IRole>>(_filepath);
        }

        public RoleManager()
        {
            _roles = new List<IRole>();
        }
    }
}
