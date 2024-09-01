using Unity.Netcode;
using UnityEngine;

namespace NGOtoGo.Examples.Spawning.PassNetworkObjectReferenceByNetworkVariable
{
    /// <summary>
    /// This is an in-scene network object spawned automatically by the network manager.
    /// When objectReference's value is changed by the host the OnValueChange event is triggered.
    /// When it spawns on the host objectReference will have a default value, on the client it will have the host's
    /// spawnObject reference.
    /// </summary>
    public class InSceneObject : NetworkBehaviour
    {
        NetworkVariable<NetworkObjectReference> objectReference = new NetworkVariable<NetworkObjectReference>();

        private void Awake()
        {
            objectReference.OnValueChanged += OnObjectReferenceChanged;            
        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            Debug.Log("InSceneObject OnNetworkSpawn objectReference initial value: " + objectReference.Value.NetworkObjectId);
        }

        private void OnObjectReferenceChanged(NetworkObjectReference oldReference, NetworkObjectReference newReference)
        {
            if(newReference.TryGet(out var spawnedNetworkObject))
            {
                Debug.Log($"InSceneObject OnObjectReferenceChanged networkObjectId: {newReference.NetworkObjectId} networkObject: {spawnedNetworkObject}");
            }
        }      

        public override void OnDestroy()
        {
            base.OnDestroy();
            
            objectReference.OnValueChanged -= OnObjectReferenceChanged;
        }

        public NetworkObjectReference ObjectReference { get => objectReference.Value; set => objectReference.Value = value; }
    }
}
