using System;
using Unity.Netcode;
using UnityEngine;

namespace NGOtoGo.Examples.Spawning.ListenForObjectSpawnEvents
{
    /// <summary>
    /// Used by the network object in place of NetworkBehaviour to allow triggering of an OnSpawn event when the object spawns.
    /// </summary>
    public abstract class EventNetworkBehaviour : NetworkBehaviour
    {   
        public static event Action<EventNetworkBehaviour> OnSpawn;

        public override void OnNetworkSpawn()
        {
            Debug.Log("EventNetworkBehaviour OnNetworkSpawn: " + NetworkObjectId);

            base.OnNetworkSpawn();

            OnSpawn?.Invoke(this);
        }
    }
}
