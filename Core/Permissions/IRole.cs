using System.Collections.Generic;

namespace Redox.Core.Permissions
{
    public interface IRole
    {
        string Name { get; }
        string Summary { get; }
        int Rank { get; }
        bool IsMasterRole { get; set; }
        HashSet<string> Groups { get; }
    }
}
