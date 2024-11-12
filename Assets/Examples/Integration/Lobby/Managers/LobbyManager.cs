using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

namespace NGOtoGo.Examples.Lobby
{
    /// <summary>
    /// Manages the lobby and lobby players.
    /// Lobby players are contained in a network list for easy sharing of player values.
    /// Local player changes are rpc'ed to the host which updates the list.
    /// List changes trigger events which are handled by their event type.
    /// Required UI changes are deferred to the UiManager.
    /// </summary>
    public class LobbyManager : NetworkBehaviour
    {
        [SerializeField] UiManager uiManager;

        NetworkList<LobbyPlayer> lobbyPlayers;
        LobbyPlayer localPlayer;

        [SerializeField] UnityEvent<List<GamePlayer>> onGameStartEvent;

        private void Awake()
        {
            Debug.Log("LobbyManager Awake");

            lobbyPlayers = new NetworkList<LobbyPlayer>();
            lobbyPlayers.OnListChanged += OnListChanged;
        }

        public override void OnNetworkSpawn()
        {
            foreach (var lobbyPlayer in lobbyPlayers)
            {
                Debug.Log("LobbyManager OnNetworkSpawn: " + lobbyPlayer);

                uiManager.AddPlayer(lobbyPlayer, lobbyPlayer.ClientId == NetworkManager.Singleton.LocalClientId);
            }
        }

        private void OnListChanged(NetworkListEvent<LobbyPlayer> changeEvent)
        {
            Debug.Log($"LobbyManager OnListChanged event: {changeEvent.Type} value: {changeEvent.Value}");

            bool isLocalPlayer = changeEvent.Value.ClientId == NetworkManager.Singleton.LocalClientId;

            switch (changeEvent.Type)
            {
                case NetworkListEvent<LobbyPlayer>.EventType.Add:
                    if(isLocalPlayer)
                    {
                        localPlayer = changeEvent.Value;
                    }

                    uiManager.AddPlayer(changeEvent.Value, isLocalPlayer);
                    UpdateStartState();
                    break;

                case NetworkListEvent<LobbyPlayer>.EventType.RemoveAt:
                    if (!isLocalPlayer)
                    {
                        uiManager.RemovePlayer(changeEvent.Value.ClientId);
                        UpdateStartState();
                    }
                    break;

                case NetworkListEvent<LobbyPlayer>.EventType.Value:
                    if (!isLocalPlayer)
                    {
                        uiManager.UpdateOtherPlayer(changeEvent.Value);
                        UpdateStartState();
                    }
                    break;

                default:
                    Debug.LogWarning("LobbyManager OnListChanged unhandled list event: " + changeEvent.Type);
                    break;
            }
        }

        public void RemovePlayer(ulong clientId)
        {
            for (int i = 0; i < lobbyPlayers.Count; i++)
            {
                if(clientId == lobbyPlayers[i].ClientId)
                {
                    lobbyPlayers.RemoveAt(i);
                }
            }
        }

        public void AddPlayer(ulong clientId, PlayerDetail playerDetail, bool isHost = false)
        {
            lobbyPlayers.Add(new LobbyPlayer(clientId, playerDetail.Name, playerDetail.Type, isHost));
        }

        // determine if the Start button should be enabled
        private void UpdateStartState()
        {
            int readyCount = 0;

            foreach (var player in lobbyPlayers)
            {
                if (player.IsReady)
                {
                    readyCount++;
                }
            }

            uiManager.UpdateStartEnable(readyCount == lobbyPlayers.Count);
        }

        // wired to UiManager onStartEvent
        public void ActionOnStart()
        {
            Debug.Log("LobbyManager ActionOnStart");

            List<GamePlayer> players = new List<GamePlayer>();

            foreach (var player in lobbyPlayers)
            {
                players.Add(new GamePlayer(player.ClientId, player.PlayerName, player.Type));
            }

            onGameStartEvent?.Invoke(players);
        }

        // wired to UiManager onPlayerTypeChangeEvent
        public void ActionOnLocalPlayerTypeChange(int type)
        {
            Debug.Log("LobbyManager ActionOnLocalPlayerTypeChange");

            localPlayer.Type = (PlayerType)type;
            LocalPlayerReadyServerRpc(localPlayer);
        }

        // wired to UiManager onPlayerReadyEvent
        public void ActionOnLocalPlayerReady()
        {
            Debug.Log("LobbyManager ActionOnLocalPlayerReady");

            localPlayer.IsReady = true;
            LocalPlayerReadyServerRpc(localPlayer);

            if (IsHost)
            {
                UpdateStartState();
            }
        }

        [Rpc(SendTo.Server)]
        public void LocalPlayerReadyServerRpc(LobbyPlayer lobbyPlayer)
        {
            Debug.Log("LobbyManager ActionOnLocalPlayerReady: " + lobbyPlayer);

            lobbyPlayers[lobbyPlayers.IndexOf(lobbyPlayer)] = lobbyPlayer;
        }

        private void OnDisable()
        {
            lobbyPlayers.OnListChanged -= OnListChanged;
        }
    }
}
