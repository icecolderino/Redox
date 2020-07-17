using Redox.Core.Permissions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Redox.API.Permissions
{
    public sealed class GroupManager : IGroupProvider
    {
        private IList<IGroup> groups;
        private readonly string _path = Path.Combine(Redox.Mod.DataPath, "redox.groups.json");
        public async Task AddPermissionAsync(string permission, string name)
        {
            IGroup group = await GetGroupAsync(name);

            if (group != null)
                if (!group.Permissions.Contains(permission))
                    group.Permissions.Add(permission);
        }

        public async Task AddPlayerAsync(ulong id, string name)
        {
            IGroup group = await GetGroupAsync(name);

            if(group != null)
            {
                if (group.Members.Contains(id))
                    group.Members.Add(id);
            }
        }

        public Task CreateGroupAsync(IGroup group)
        {
            if (group != null)
                if (!groups.Any(x => x.Name == group.Name))
                    groups.Add(group);

            return Task.CompletedTask;

        }

        public Task<IGroup> GetGroupAsync(string name)
        {
            IGroup group = groups.FirstOrDefault(x => x.Name == name);
            return Task.FromResult(group);
        }

        public Task<IEnumerable<IGroup>> GetGroupsAsync()
        {
            return Task.FromResult(groups.AsEnumerable());
        }

        public Task<IEnumerable<IGroup>> GetPlayerGroupsAsync(ulong id)
        {
            IEnumerable<IGroup> sequence = (from x in groups
                                            where x.Members.Contains(id)
                                            select x);
            return Task.FromResult(sequence);
        }

        public async Task RemoveGroupAsync(string name)
        {
            IGroup group = await GetGroupAsync(name);
            if (group != null)
                groups.Remove(group);
        }

        public async Task RemovePermissionAsync(string permission, string name)
        {
            IGroup group = await GetGroupAsync(name);

            if(group != null)
                if (group.Permissions.Contains(permission))
                    group.Permissions.Remove(permission);
           
        }

        public async Task RemovePlayerAsync(ulong id, string name)
        {
            IGroup group = await GetGroupAsync(name);

            if (group != null)
                if (group.Members.Contains(id))
                    group.Members.Remove(id);
        }

        public async Task LoadAsync()
        {
            await Utility.Json.ToFileAsync(_path, groups);
        }

     

        public async Task SaveAsync()
        {
            groups = await Utility.Json.FromFileAsync<List<IGroup>>(_path);
        }
    }
}
