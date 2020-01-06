using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Redox.API.Libraries
{
    public static class Groups
    {
        private static HashSet<Group> _groups = new HashSet<Group>();

        public static async Task Load()
        {
            Redox.Logger.Log("[Redox] Loading groups..");
            _groups = await JSONHelper.FromFileAsync<HashSet<Group>>(Path.Combine(Redox.DataPath, "groups.json"));
        }

        public static async Task Save()
        {
            await JSONHelper.ToFileAsync(Path.Combine(Redox.DataPath, "groups.json"), _groups);
        }

        public static Group CreateGroup(string Name)
        {
            if(!Exists(Name))
            {
                Group group = new Group();
                group.Name = Name;
                _groups.Add(group);
                return group; 
            }
            return GetGroup(Name);
        }
        public static Group GetGroup(string name)
        {
            return _groups.FirstOrDefault(x => x.Name == name);            
        }
        public static bool Exists(string name)
        {
            return _groups.Any(x => x.Name == name);
        }


        public static void AddUser(string Group, string steamID)
        {
            if(Exists(Group))
                GetGroup(Group).Users.Add(steamID);
        }

        public static void RemoveUser(string Group, string steamID)
        {
            if (Exists(Group))
                GetGroup(Group).Users.Remove(steamID);
        }

        public static void AddPermission(string Group, string permission)
        {
            if (Exists(Group))
            {
                GetGroup(Group).Permissions.Add(permission);
            }
        }

        public static bool InGroup(string Group, string steamID)
        {
            return GetGroup(Group)?.Users.Contains(steamID) ?? false;
        }

        public static void RemovePermission(string Group, string permission)
        {
            if (Exists(Group))
            {
                GetGroup(Group).Permissions.Remove(permission);
            }
        }

        public static bool HasPermission(string Group, string permission)
        {
            if (Exists(Group))
            {
                return GetGroup(Group).Permissions.Contains(permission);
            }
            return false;
        }

        public static IEnumerable<string> GetPlayerGroups(string steamID)
        {
            var groups = _groups.Where(x => x.Users.Contains(steamID)).Select(x => x.Name);
            return groups;
        }
    }

    [Serializable]
    public class Group
    {
        /// <summary>
        /// The name of the Group
        /// </summary>
        public string Name = "Unnamed";

        /// <summary>
        /// List of users inside the group
        /// </summary>
        public HashSet<string> Users = new HashSet<string>();

        /// <summary>
        /// List of registered permissions in the group
        /// </summary>
        public HashSet<string> Permissions = new HashSet<string>(); 
       
    }

}
