using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Redox.API.Libraries
{
    public static class Groups
    {
        private static HashSet<Group> _groups = new HashSet<Group>();

        public static void Load()
        {
            if(DataStore.GetInstance().ContainsKey("Redox", "Groups"))
            {
                _groups = (HashSet<Group>)DataStore.GetInstance().GetValue("Redox", "Groups");
            }
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
