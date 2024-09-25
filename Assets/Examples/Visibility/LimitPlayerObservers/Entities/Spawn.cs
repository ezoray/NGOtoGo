using Unity.Netcode;
using UnityEngine;

namespace NGOtoGo.Examples.Visibility.LimitPlayerObservers
{
    public class Spawn : NetworkBehaviour
    {

        public override void OnNetworkSpawn()
        {
            Debug.Log("Spawn OnNetworkSpawn clientId: " + OwnerClientId);
        }
    }
}
