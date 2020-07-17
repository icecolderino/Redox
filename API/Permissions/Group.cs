using Redox.Core.Permissions;
using System.Collections.Generic;

namespace Redox.API.Permissions
{
    public sealed class Group : IGroup
    {  
        public string Name { get; }

        public string Summary { get; }

        public int Rank { get; }

        public string Role { get; }

        public bool AutoAssign { get; set; }
        public bool IsMasterGroup { get; set; }
        public HashSet<ulong> Members { get; }

        public HashSet<string> Permissions { get; }
     
        public Group(string name, string summary, string role, int rank, bool autoassign, bool mastergroup)
        {
            Name = name;
            Summary = summary;
            Rank = rank;
            Role = role;
            AutoAssign = autoassign;
            IsMasterGroup = mastergroup;
            Members = new HashSet<ulong>();
            Permissions = new HashSet<string>();
        }
    }
}
