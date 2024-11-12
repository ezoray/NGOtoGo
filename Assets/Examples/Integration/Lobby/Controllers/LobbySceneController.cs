using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NGOtoGo.Examples.Lobby
{
    /// <summary>
    /// This scene handles the adding and removing of players to/from the lobby.
    /// On Start the lobby player data is entered into the game detail scriptable object so it's available to the Game scene.
    /// </summary>
    public class LobbySceneController : MonoBehaviour
    {    
        [SerializeField] LobbyManager lobbyManager;
        [SerializeField] GameDetailSO gameDetail;

        NetworkManager networkManager;
        ConnectionApprovalManager approvalManager; // contains the player data for approved clients

        private void OnEnable()
        {
            networkManager = NetworkManager.Singleton;
            approvalManager = networkManager.GetComponent<ConnectionApprovalManager>();            
        }

        void Start()
        {
            Debug.Log("LobbySceneController Start isHost: " + networkManager.IsHost);

            if (networkManager.IsHost)
            {                
                networkManager.OnClientConnectedCallback += OnClientConnected;
                networkManager.OnClientDisconnectCallback += OnClientDisconnected;

                // if we got this far we know the approvalManager contains the host player data
                if(approvalManager.TryGetJoinedPlayer(networkManager.LocalClientId, out var playerDetail))
                {
                    lobbyManager.AddPlayer(networkManager.LocalClientId, playerDetail, true);
                }                
            }
        }

        private void OnClientDisconnected(ulong clientId)
        {
            if(clientId != NetworkManager.ServerClientId)
            {
                lobbyManager.RemovePlayer(clientId);
            }
        }

        // handles the connection of clients but not host as their connection has already been handled in the Menu scene
        private void OnClientConnected(ulong clientId)
        {
            Debug.Log("LobbySceneController OnClientConnected clientId: " + clientId);

            if (approvalManager.TryGetJoinedPlayer(clientId, out var playerDetail))
            {
                lobbyManager.AddPlayer(clientId, playerDetail);
            }
        }

        // wired to LobbyManager onGameStartEvent
        public void ActionOnGameStart(List<GamePlayer> gamePlayers)
        {
            gameDetail.GamePlayers = gamePlayers;

            networkManager.SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
        }

        private void OnDisable()
        {
            if (networkManager.IsHost)
            {                
                networkManager.OnClientConnectedCallback -= OnClientConnected;
                networkManager.OnClientDisconnectCallback -= OnClientDisconnected;
            }
        }
    }
}
