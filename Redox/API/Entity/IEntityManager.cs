
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Redox.API.Player;


using UnityEngine;

namespace Redox.API.Entity
{
    public interface IEntityManager
    {
        IList<IEntity> Entities { get; }

        IEntity GetEntityAt(Vector3 location);

        IEnumerable<IEntity> GetEntitiesOfType<T>(T obj);

        IEnumerable<IEntity> GetEntitiesOfPlayer(string id);

        IEnumerable<IEntity> GetEntitiesOfPlayer(IPlayer player);

        IEntity GetClosestEntity(Vector3 location, float radius);

        void DestroyAll();

    }

}

