using Unity.Netcode;
using UnityEngine;

namespace NGOtoGo.Examples.Events.SubscribeToNetworkEvents
{
    /// <summary>
    /// Logs network events associated with the network object
    /// </summary>
    public class Player : NetworkBehaviour
    {
        public override void OnNetworkSpawn()
        {
            Debug.Log($"Player clientId: {OwnerClientId} OnNetworkSpawn");
        }

        private void Start()
        {
            Debug.Log($"Player clientId: {OwnerClientId} Start");
        }

        protected override void OnSynchronize<T>(ref BufferSerializer<T> serializer)
        {
            Debug.Log($"Player clientId: {OwnerClientId} OnSynchronize");
        }

        protected override void OnNetworkSessionSynchronized()
        {
            Debug.Log($"Player clientId: {OwnerClientId} OnNetworkSessionSynchronized");
        }

        protected override void OnNetworkPreSpawn(ref NetworkManager networkManager)
        {
            Debug.Log($"Player clientId: {OwnerClientId} OnNetworkPreSpawn");
        }

        protected override void OnNetworkPostSpawn()
        {
            Debug.Log($"Player clientId: {OwnerClientId} OnNetworkPostSpawn");
        }

        public override void OnNetworkDespawn()
        {
            Debug.Log($"Player clientId: {OwnerClientId} OnNetworkDespawn");
        }

        public override void OnDestroy()
        {
            Debug.Log($"Player clientId: {OwnerClientId} OnDestroy");

            base.OnDestroy();
        }
    }
}
