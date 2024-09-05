using Unity.Netcode;
using UnityEngine;

namespace NGOtoGo.Examples.Connection.PassChosenPlayerOnConnection
{
    /// <summary>
    /// Player object used by host.
    /// </summary>
    public class MalePlayer : NetworkBehaviour
    {
        public override void OnNetworkSpawn()
        {
            Debug.Log($"MalePlayer OnNetworkSpawn networkObjectId: {NetworkObjectId}");

            base.OnNetworkSpawn();
        }
    }
}
