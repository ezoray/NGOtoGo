using Unity.Netcode;
using UnityEngine;

namespace NGOtoGo.Examples.Spawning.ListenForObjectSpawnEvents
{
    /// <summary>
    /// Spawns an object upon each connection which is picked up by the SpawnListener.
    /// </summary>
    public class SceneController : MonoBehaviour
    {
        [SerializeField] NetworkObject spawnObjectPrefab;
        NetworkManager networkManager;

        private void Start()
        {
            Application.targetFrameRate = 15; // arbitrary framerate limit

            networkManager = NetworkManager.Singleton;

            if (!ParrelSync.ClonesManager.IsClone())
            {
                networkManager.OnClientConnectedCallback += OnHostClientConnected;
                networkManager.StartHost();
            }
            else
            {                
                networkManager.StartClient();
            }
        }

        private void OnHostClientConnected(ulong clientId)
        {
            networkManager.SpawnManager.InstantiateAndSpawn(spawnObjectPrefab);
        }

        private void OnDestroy()
        {
            networkManager.OnClientConnectedCallback -= OnHostClientConnected;
        }
    }
}
