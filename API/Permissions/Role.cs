using Redox.Core.Permissions;
using System.Collections.Generic;

namespace Redox.API.Permissions
{
    public sealed class Role : IRole
    {
        public string Name { get; }
        public string Summary { get; }

        public int Rank { get; }
        public bool DefaultRole { get; set; }
        public bool IsMasterGroup { get; set; }
        public HashSet<ulong> Members { get; }

        public HashSet<string> Permissions { get; }
     
        public Role(string name, string summary, string role, int rank, bool defaultrole, bool mastergroup)
        {
            Name = name;
            Summary = summary;
            Rank = rank;
            DefaultRole = defaultrole;
            IsMasterGroup = mastergroup;
            Members = new HashSet<ulong>();
            Permissions = new HashSet<string>();
        }
    }
}
