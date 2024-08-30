using System;
using Unity.Netcode;
using UnityEngine;

namespace NGOtoGo.Examples.LocalPlayer.GetLocalPlayerOnConnection
{
    /// <summary>
    /// Gets the Player object upon connection.
    /// I've used separate OnClientConnectedCallbacks for host and client to make the code is easier to follow.
    /// </summary>
    public class SceneController : MonoBehaviour
    {
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
                networkManager.OnClientConnectedCallback += OnClientConnected;
                networkManager.StartClient();
            }
        }

        private void OnHostClientConnected(ulong clientId)
        {
            if (clientId == NetworkManager.ServerClientId)
            {
                Player player = networkManager.LocalClient.PlayerObject.GetComponent<Player>();

                Debug.Log($"SceneController OnHostClientConnected Host's clientId: {clientId} player networkObjectId: {player.NetworkObjectId}");
            }
        }

        private void OnClientConnected(ulong clientId)
        {            
            Player player = networkManager.LocalClient.PlayerObject.GetComponent<Player>();

            Debug.Log($"SceneController OnClientConnected clientId: {clientId} player networkObjectId: {player.NetworkObjectId}");
        }

        private void OnDestroy()
        {
            if (networkManager.IsHost)
            {
                networkManager.OnClientConnectedCallback -= OnHostClientConnected;
            }
            else
            {
                networkManager.OnClientConnectedCallback -= OnClientConnected;
            }

            networkManager.Shutdown();
        }
    }
}

