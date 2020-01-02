
using System;

using Redox.API.Player;

using UnityEngine;

namespace Redox.API.Entity
{
    public interface IEntity
    {
        string Name { get; }

        string OwnerName { get; }

        string CreatorName { get; }

        string OwnerID { get; }

        string CreatorID { get; }

        ulong OwnerUID { get; }

        ulong CreatorUID { get; }

        IPlayer Owner { get; }

        IPlayer Creator { get; }

        float Health { get; set; }

        float MaxHealth { get; }

        int UniqueID { get; }

        object Object { get; }

        IEntity OriginalEntity { get; }

        T GetObject<T>();

        Vector3 Location { get; }

        Quaternion Rotation { get; }

        void Destroy();


    }
}
