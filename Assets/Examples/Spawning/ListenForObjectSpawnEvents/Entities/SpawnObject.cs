using Unity.Netcode;
using UnityEngine;

namespace NGOtoGo.Examples.Spawning.ListenForObjectSpawnEvents
{
    /// <summary>
    /// Network object to spawn, base.OnNetworkSpawn must be called to invoke an OnSpawn event.
    /// </summary>
    public class SpawnObject : EventNetworkBehaviour
    {
        public override void OnNetworkSpawn()
        {
            Debug.Log($"SpawnObject OnNetworkSpawn networkObjectId: {NetworkObjectId}");

            base.OnNetworkSpawn();
        }
    }
}
