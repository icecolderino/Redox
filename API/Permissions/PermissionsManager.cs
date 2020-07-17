using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Redox.API.Player;
using Redox.Core.Permissions;

namespace Redox.API.Permissions
{
    public sealed class PermissionsManager : IPermissionProvider
    {
        private IDictionary<ulong, IList<string>> permissions;

        private readonly string _path = Path.Combine(Redox.Mod.DataPath, "redox.permissions.json");

        public Task<IEnumerable<string>> GetPlayerPermissionsAsync(IPlayer player)
        {
            ulong id = player.UID;

            return Task.FromResult(permissions[id].AsEnumerable());
        }

        public Task GrantPermissionAsync(IPlayer player, string permission)
        {
            var perms = permissions[player.UID];
            if (!perms.Contains(permission))
                perms.Add(permission);
            return Task.CompletedTask;
        }

        public Task RevokePermission(IPlayer player, string permission)
        {
            var perms = permissions[player.UID];
            if (perms.Contains(permission))
                perms.Remove(permission);
            return Task.CompletedTask;
        }

        public async Task SaveAsync()
        {
            await Utility.Json.ToFileAsync(_path, permissions);
        }
        public async Task LoadAsync()
        {
            permissions = await Utility.Json.FromFileAsync<Dictionary<ulong, IList<string>>>(_path);
        }
    }
}
