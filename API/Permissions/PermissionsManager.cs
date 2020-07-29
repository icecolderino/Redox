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
        private IDictionary<ulong, IList<string>> _permissions;

        private readonly string _filepath = Path.Combine(Redox.Mod.DataPath, "redox.permissions.json");


        public bool Exists => File.Exists(_filepath);

        public Task<IEnumerable<string>> GetPlayerPermissionsAsync(IPlayer player)
        {
            ulong id = player.Uid;

            return Task.FromResult(_permissions[id].AsEnumerable());
        }

        public Task GrantPermissionAsync(IPlayer player, string permission)
        {
            var perms = _permissions[player.Uid];
            if (!perms.Contains(permission))
                perms.Add(permission);
            return Task.CompletedTask;
        }

        public Task RevokePermission(IPlayer player, string permission)
        {
            var perms = _permissions[player.Uid];
            if (perms.Contains(permission))
                perms.Remove(permission);
            return Task.CompletedTask;
        }
        public async Task SaveAsync()
        {
            await Utility.Json.ToFileAsync(_filepath, _permissions);
        }
        public async Task LoadAsync()
        {
            _permissions = await Utility.Json.FromFileAsync<Dictionary<ulong, IList<string>>>(_filepath);
        }
    }
}
