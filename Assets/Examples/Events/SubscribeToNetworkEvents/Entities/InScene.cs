using Unity.Netcode;
using UnityEngine;

namespace NGOtoGo.Examples.Events.SubscribeToNetworkEvents
{
    /// <summary>
    /// Logs network events associated with the network object
    /// </summary>
    public class InScene : NetworkBehaviour
    {
        public override void OnNetworkSpawn()
        {
            Debug.Log($"InScene OnNetworkSpawn");
        }

        private void Start()
        {
            Debug.Log($"InScene Start");
        }

        public override void OnGainedOwnership()
        {
            Debug.Log($"InScene OnGainedOwnership{(IsHost ? " (always called on host even if they lose ownership)" : "")}");
        }

        public override void OnLostOwnership()
        {
            Debug.Log($"InScene OnLostOwnership");
        }

        protected override void OnInSceneObjectsSpawned()
        {
            Debug.Log($"InScene OnInSceneObjectsSpawned");
        }

        protected override void OnSynchronize<T>(ref BufferSerializer<T> serializer)
        {
            Debug.Log($"InScene OnSynchronize");
        }

        protected override void OnNetworkSessionSynchronized()
        {
            Debug.Log($"InScene OnNetworkSessionSynchronized");
        }

        protected override void OnNetworkPreSpawn(ref NetworkManager networkManager)
        {
            Debug.Log($"InScene OnNetworkPreSpawn");
        }

        protected override void OnNetworkPostSpawn()
        {
            Debug.Log($"InScene OnNetworkPostSpawn");
        }

        public override void OnNetworkDespawn()
        {
            Debug.Log($"InScene OnNetworkDespawn");
        }

        public override void OnDestroy()
        {
            Debug.Log($"InScene OnDestroy");

            base.OnDestroy();
        }
    }
}
