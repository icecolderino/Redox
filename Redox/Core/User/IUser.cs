using System;


namespace Redox.Core.User
{
    public interface IUser
    {
        string Name { get; }

        DateTime? lastSeen { get; }
    }
}
