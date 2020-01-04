using System;
using System.Collections.Generic;


namespace Redox.API.Libraries
{
    public static class Permissions
    {
        private static Dictionary<string, UserData> _users = new Dictionary<string, UserData>();

        public static void Load()
        {
            if(DataStore.GetInstance().ContainsKey("Redox", "Permissions"))
            {
                _users = (Dictionary<string, UserData>)DataStore.GetInstance().GetValue("Redox", "Permissions");
            }


        }
    }

    [Serializable]
    public class UserData
    {
        public HashSet<string> Permissions = new HashSet<string>();

        public HashSet<string> Groups = new HashSet<string>();
    }
}
