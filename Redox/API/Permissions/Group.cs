
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Redox.API.Permissions
{
    public sealed class Group
    {
        [JsonProperty]
        public string Name { get; internal set; } = "Unnamed";

        [JsonProperty]
        public HashSet<string> Permissions { get; internal set; } = new HashSet<string>();

        [JsonProperty]
        public HashSet<ulong> Users { get; internal set; } = new HashSet<ulong>();

        public void AddUser(ulong uid)
        {
            if (!Users.Contains(uid))
                Users.Add(uid);
        }
        public void RemoveUser(ulong uid)
        {
            if (Users.Contains(uid))
                Users.Remove(uid);
        }

        public void AssignPermission(string permission)
        {
            if (!Permissions.Contains(permission))
                Permissions.Add(permission);
        }
        public void UnassignPermission(string permission)
        {
            if (Permissions.Contains(permission))
                Permissions.Remove(permission);
        }

    }
}