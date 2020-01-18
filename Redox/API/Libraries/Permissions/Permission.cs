using System;
using System.Collections.Generic;

using Newtonsoft.Json;

namespace Redox.API.Permissions
{   
    public sealed class Permission
    {
        [JsonProperty]
        public ulong userID { get; set; }

        [JsonProperty]
        public HashSet<string> Permissions { get; internal set; }

        [JsonProperty]
        public HashSet<string> Groups { get; internal set; }

        

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
        public bool HasPermission(string permission)
        {
            return Permissions.Contains(permission);
        }

        public void JoinGroup(string group)
        {
            Group Group = PermissionManager.GetGroup(group);

            if(Group != null)
            {
                if(!Groups.Contains(group))
                {
                    Groups.Add(group);
                    Group.AddUser(userID);
                }
            }
        }
        public void LeaveGroup(string group)
        {
            Group Group = PermissionManager.GetGroup(group);

            if (Group != null)
            {
                if(Groups.Contains(group))
                {
                    Groups.Remove(group);
                    Group.RemoveUser(userID);
                }
            }
        }  
        public bool InGroup(string group)
        {
            return Groups.Contains(group);
        }

        public Permission(ulong ID)
        {
            userID = ID;
            Permissions = new HashSet<string>();
            Groups = new HashSet<string>();
        }
    }
}