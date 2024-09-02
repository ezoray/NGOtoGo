using System;
using System.Runtime.Serialization;
using Unity.Netcode;
using UnityEngine;

namespace NGOtoGo.Examples.Spawning.PassNetworkBehaviourReferenceByNetworkVariable
{
    /// <summary>
    /// This is an in-scene network object spawned automatically by the network manager.
    /// When behaviourReference's value is changed by the host the OnValueChange event is triggered.
    /// </summary>
    public class InSceneObject : NetworkBehaviour
    {        
        NetworkVariable<NetworkBehaviourReference> behaviourReference = new NetworkVariable<NetworkBehaviourReference>();

        private void Awake()
        {
            behaviourReference.OnValueChanged += OnBehaviourReferenceChanged;
        }

        private void OnBehaviourReferenceChanged(NetworkBehaviourReference oldReference, NetworkBehaviourReference newReference)
        {
            if (newReference.TryGet(out NetworkBehaviour spawnedObject))
            {
                Debug.Log($"InSceneObject OnBehaviourReferenceChanged networkObjectId: {spawnedObject.NetworkObjectId} spawnedObject: {spawnedObject}");
            }
        }

        public override void OnDestroy()
        {
            base.OnDestroy();

            behaviourReference.OnValueChanged -= OnBehaviourReferenceChanged;            
        }

        public NetworkBehaviourReference BehaviourReference { get => behaviourReference.Value; set => behaviourReference.Value = value; }
    }
}
