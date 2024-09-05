using Unity.Netcode;
using UnityEngine;

namespace NGOtoGo.Examples.Connection.PassChosenPlayerOnConnection
{
    /// <summary>
    /// Player object used by client.
    /// </summary>
    public class FemalePlayer : NetworkBehaviour
    {        
        public override void OnNetworkSpawn()
        {
            Debug.Log($"FemalePlayer OnNetworkSpawn networkObjectId: {NetworkObjectId}");

            base.OnNetworkSpawn();
        }
    }
}
