
using System.Collections.Generic;

namespace Redox.Core.Permissions
{
    public interface IRole
    {
        /// <summary>
        /// The name of the group.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Brief description.
        /// </summary>
        string Summary { get; }

        /// <summary>
        /// The rank of this group.
        /// </summary>
        int Rank { get; }

        /// <summary>
        /// Add players automatically to this group?
        /// </summary>
        bool DefaultRole { get; set; }

        /// <summary>
        /// Determines if its a master group.
        /// </summary>
        bool IsMasterGroup { get; set; }

        /// <summary>
        /// List of members.
        /// </summary>
        HashSet<ulong> Members { get; }
        /// <summary>
        /// List of permissions.
        /// </summary>
        HashSet<string> Permissions { get; }
    }
}
