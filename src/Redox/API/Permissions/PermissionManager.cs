
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Redox.API.Helpers;

namespace Redox.API.Permissions
{
    public static class PermissionManager
    {
        private static IDictionary<ulong, Permission> _permissions;
        private static HashSet<Group> _groups;

        private static string _permsPath = Path.Combine(Redox.DataPath, "redox_permissions.json");
        private static string _groupsPath = Path.Combine(Redox.DataPath, "redox_groups.json");

        public static async Task Initialize()
        {
            await Task.Run(() =>
            {
                _permissions = JSONHelper.FromFile<Dictionary<ulong, Permission>>(_permsPath) ?? new Dictionary<ulong, Permission>();
                _groups = JSONHelper.FromFile<HashSet<Group>>(_groupsPath) ?? new HashSet<Group>();
            });
        }

        public static async Task Save()
        {
            await JSONHelper.ToFileAsync(_permsPath, _permissions);
            await JSONHelper.ToFileAsync(_groupsPath, _groups);
        }

        /// <summary>
        /// Gets the permissions of the player
        /// </summary>
        /// <param name="playerUID"></param>
        /// <returns>Permission data</returns>
        public static Permission GetPermissionData(ulong playerUID)
        {
            Permission permission;

            if (_permissions.TryGetValue(playerUID, out permission))
                return permission;
            permission = new Permission(playerUID);
            _permissions.Add(playerUID, permission);
            return permission;
        }

        public static IEnumerable<Group> GetGroupsFromPlayer(ulong playerUID)
        {
            return (from x in _groups
                    where x.Users.Contains(playerUID)
                    select x
                    );
        }

        public static Group CreateGroup(string name)
        {
            if(!_groups.Any(x => x.Name == name))
            {
                Group group = new Group();
                group.Name = name;
                _groups.Add(group);
                return group;
            }
            return _groups.SingleOrDefault(x => x.Name == name);
        }

        public static void RemoveGroup(string name)
        {
            Group group = _groups.FirstOrDefault(x => x.Name == name);
            if (group != null)
                _groups.Remove(group);
        }
        public static Group GetGroup(string name)
        {
            return _groups.FirstOrDefault(x => x.Name == name);
        }
    }
}   
