using Redox.Core.Permissions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Redox.API.Permissions
{
    public sealed class Role : IRole
    {
        public string Name { get; }

        public string Summary { get; }

        public int Rank { get; }
        public bool IsMasterRole { get; set; }

        public HashSet<string> Groups { get; }

        public Role(string name, string summary, int rank,  bool isMasterRole)
        {
            Name = name;
            Summary = summary;
            Rank = rank;
            IsMasterRole = isMasterRole;
            Groups = new HashSet<string>();
        }
    }
}
