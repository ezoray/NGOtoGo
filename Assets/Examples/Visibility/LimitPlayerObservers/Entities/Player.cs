using Unity.Netcode;
using UnityEngine;

namespace NGOtoGo.Examples.Visibility.LimitPlayerObservers
{
    /// <summary>
    /// Player object for each client.
    /// Untick 'Spawn With Observers' on the prefab's Network Object so object only spawns on server/host.
    /// </summary>
    public class Player : NetworkBehaviour
    {

        public override void OnNetworkSpawn()
        {
            Debug.Log($"Player OnNetworkSpawn localClientId: {NetworkManager.LocalClientId} ownerClientId: {OwnerClientId} IsLocalPlayer: {IsLocalPlayer}");
        }
    }
}
