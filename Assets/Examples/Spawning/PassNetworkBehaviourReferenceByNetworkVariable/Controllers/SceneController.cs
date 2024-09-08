using Unity.Netcode;
using UnityEngine;

namespace NGOtoGo.Examples.Spawning.PassNetworkBehaviourReferenceByNetworkVariable
{
    /// <summary>
    /// When the host/client connects a SpawnObject is spawned and the InSceneObject's BehaviourReference field is updated
    /// triggering an OnValueChanged event.
    /// Due to clients connecting later than the host the BehaviourReference field will already be set with the host's BehaviourReference
    /// when InSceneObject spawns on the client, the OnValueChanged event is not triggered for this.
    /// </summary>
    public class SceneController : MonoBehaviour
    {
        NetworkManager networkManager;
        [SerializeField] InSceneObject inSceneObject;
        [SerializeField] NetworkObject spawnPrefab;

        private void Start()
        {
            Application.targetFrameRate = 15; // arbitrary framerate limit

            networkManager = NetworkManager.Singleton;

            if (!ParrelSync.ClonesManager.IsClone())
            {
                networkManager.OnClientConnectedCallback += OnClientConnected;
                networkManager.StartHost();
            }
            else
            {
                networkManager.StartClient();
            }
        }

        private void OnClientConnected(ulong clientId)
        {
            NetworkObject spawnNetworkObject = networkManager.SpawnManager.InstantiateAndSpawn(spawnPrefab);

            if (spawnNetworkObject.TryGetComponent<SpawnObject>(out var spawnObject))
            {
                inSceneObject.BehaviourReference = spawnObject;
            }
        }

        private void OnDestroy()
        {
            if (networkManager.IsHost)
            {
                networkManager.OnClientConnectedCallback -= OnClientConnected;
            }
        }
    }
}
