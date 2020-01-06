using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Redox.API.Libraries
{
    public static class Permissions
    {
        private static Dictionary<string, UserData> _users = new Dictionary<string, UserData>();

        public static async Task Load()
        {
            Redox.Logger.Log("[Redox] Loading permissions..");
            _users = await JSONHelper.FromFileAsync<Dictionary<string, UserData>>(Path.Combine(Redox.DataPath, "permissions.json"));
        }

        public static async Task Save()
        {
            await JSONHelper.ToFileAsync(Path.Combine(Redox.DataPath, "permissions.json"), _users);
        }

        /// <summary>
        /// Checks if the user is in the list, otherwise it adds him
        /// </summary>
        /// <param name="steamID"></param>
        public static void CheckUser(string steamID)
        {
            if (!_users.ContainsKey(steamID))
                _users.Add(steamID, new UserData());
        }

        public static void RegisterPermission(string steamID, string permission)
        {
            UserData data;
            if (_users.TryGetValue(steamID, out data))
            {
                if (!data.Permissions.Contains(permission))
                    data.Permissions.Add(permission);
                _users[steamID] = data;
            }

        }

        public static bool HasPermission(string steamID, string permission)
        {
            return _users[steamID]?.Permissions.Contains(permission) ?? false;
        }

        public static void UnregisterPermission(string steamID, string permission)
        {
            UserData data;

            if (_users.TryGetValue(steamID, out data))
            {
                if (data.Permissions.Contains(permission))
                    data.Permissions.Remove(permission);
                _users[steamID] = data;
            }
        }

        public static HashSet<string> GetPermissions(string steamID)
        {
            return _users[steamID]?.Permissions ?? new HashSet<string>();
        }
    }

    [Serializable]
    public class UserData
    {
        public HashSet<string> Permissions = new HashSet<string>();

    }
}
