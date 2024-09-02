using Unity.Netcode;
using UnityEngine;

namespace NGOtoGo.Examples.Spawning.PassNetworkBehaviourReferenceByNetworkVariable
{
    /// <summary>
    /// Network object to spawn on the each client connection.
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
