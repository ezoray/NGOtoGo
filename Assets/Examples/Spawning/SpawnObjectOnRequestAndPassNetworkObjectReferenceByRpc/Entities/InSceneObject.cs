using Unity.Netcode;
using UnityEngine;

namespace NGOtoGo.Examples.Spawning.SpawnObjectOnRequestAndPassNetworkObjectReferenceByRpc
{
    /// <summary>
    /// An in-scene object that handles the RPC's to spawn the object and send the NetworkObjectReference.
    /// </summary>
    public class InSceneObject : NetworkBehaviour
    {
        [SerializeField] NetworkObject spawnObjectPrefab;

        [Rpc(SendTo.Everyone)]
        public void SendObjectReferenceRpc(NetworkObjectReference objectReference)
        {
            if (objectReference.TryGet(out var spawnNetworkObject))
            {
                if (spawnNetworkObject.TryGetComponent<SpawnObject>(out var spawnObject))
                {
                    Debug.Log($"InSceneObject SendObjectReferenceRpc: networkObjectId: {spawnObject.NetworkObjectId} spawnObject: {spawnObject}");
                }
            }
        }

        [Rpc(SendTo.Server)]
        public void SpawnObjectRpc()
        {
            NetworkObject spawnNetworkObject = NetworkManager.SpawnManager.InstantiateAndSpawn(spawnObjectPrefab);

            SendObjectReferenceRpc(spawnNetworkObject);

        }
    }
}
