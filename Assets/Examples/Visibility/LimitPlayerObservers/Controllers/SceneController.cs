using System;
using Unity.Netcode;
using UnityEngine;

namespace NGOtoGo.Examples.Visibility.LimitPlayerObservers
{
    /// <summary>
    /// Have only the client's Player object be spawned on the client.
    /// Unticking 'Spawn With Observers' on the Player prefab's Network Object prevents automatic spawning on all clients.
    /// Bug - In NGO 2.x Player objects of other clients are spawned on the client.
    /// </summary>
    public class SceneController : MonoBehaviour
    {
        NetworkManager networkManager;

        private void Start()
        {
            Application.targetFrameRate = 15;

            networkManager = NetworkManager.Singleton;

            if (!ParrelSync.ClonesManager.IsClone())
            {                 
                networkManager.OnClientConnectedCallback += OnClientConnected;

                // use StartServer if running as server only (no host player)
                networkManager.StartHost();
            }
            else
            {
                networkManager.StartClient();
            }
        }

        private void OnClientConnected(ulong clientId)
        {
            // remove this check if using StartServer
            if (clientId != NetworkManager.ServerClientId)
            {
                if (networkManager.ConnectedClients.TryGetValue(clientId, out NetworkClient networkClient))
                {
                    // spawns the Player on the client / makes the client an observer of this object
                    networkClient.PlayerObject.NetworkShow(clientId);
                }
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
