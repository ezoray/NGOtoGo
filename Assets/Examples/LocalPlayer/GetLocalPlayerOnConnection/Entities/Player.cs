using Unity.Netcode;
using UnityEngine;

namespace NGOtoGo.Examples.LocalPlayer.GetLocalPlayerOnConnection
{
    public class Player : NetworkBehaviour
    {
        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            Debug.Log($"Player OnNetworkSpawn, client: {OwnerClientId} networkObjectId: {NetworkObjectId}");
        }
    }
}
