using Unity.Netcode;
using UnityEngine;

namespace NGOtoGo.Examples.Spawning.SpawnObjectOnRequestAndPassNetworkObjectReferenceByRpc
{
    /// <summary>
    /// Network object to spawn on receipt of RPC from host or client.
    /// </summary>
    public class SpawnObject : NetworkBehaviour
    {
        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            Debug.Log($"SpawnObject OnNetworkSpawn networkObjectId: {NetworkObjectId}");
        }
    }
}
