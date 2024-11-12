using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace NGOtoGo.Examples.Lobby
{
    /// <summary>
    /// This class handles the UI side of the lobby.
    /// Players added to the lobby are instantiated from prefabs with the local player having their own as they have
    /// interactable elements.
    /// Changes by the local player trigger unity events subscribed to by the LobbyManager.
    /// </summary>
    public class UiManager : MonoBehaviour
    {
        [SerializeField] LocalPlayer localPlayerPrefab;
        [SerializeField] OtherPlayer otherPlayerPrefab;
        [SerializeField] RectTransform playerPanel;
        [SerializeField] Button startButton;

        [SerializeField] UnityEvent<int> onPlayerTypeChangeEvent;
        [SerializeField] UnityEvent onPlayerReadyEvent;
        [SerializeField] UnityEvent onStartEvent;

        Dictionary<ulong, OtherPlayer> otherPlayers;
        LocalPlayer localPlayer;

        private void Awake()
        {
            Debug.Log("UiManager Awake");
            otherPlayers = new Dictionary<ulong, OtherPlayer>();

            startButton.interactable = false;
            startButton.gameObject.SetActive(false);
        }

        public void UpdateStartEnable(bool canStart)
        {
            startButton.interactable = canStart;
        }

        public void RemovePlayer(ulong clientId)
        {
            if(otherPlayers.TryGetValue(clientId, out var otherPlayer))
            {
                otherPlayers.Remove(clientId);
                Destroy(otherPlayer.gameObject);
            }
        }

        public void AddPlayer(LobbyPlayer lobbyPlayer, bool isLocalPlayer)
        {
            Debug.Log($"UiManager AddPlayer isLocalPlayer: {isLocalPlayer} lobbyPlayer: {lobbyPlayer}");

            if(isLocalPlayer)
            {
                AddLocalPlayer(lobbyPlayer);
            }
            else
            {
                AddOtherPlayer(lobbyPlayer);
            }
        }

        public void UpdateOtherPlayer(LobbyPlayer lobbyPlayer)
        {
            if (otherPlayers.TryGetValue(lobbyPlayer.ClientId, out var otherPlayer))
            {
                otherPlayer.TypeText.text = lobbyPlayer.Type.ToString();
                otherPlayer.ReadyText.text = lobbyPlayer.IsReady ? "Ready" : "Not Ready";
            }
        }

        private void AddOtherPlayer(LobbyPlayer lobbyPlayer)
        {
            OtherPlayer otherPlayer = Instantiate(otherPlayerPrefab);

            //     otherPlayer.BgImage.color = lobbyPlayer.IsHost ? Color.blue : Color.green;
            otherPlayer.IdText.text = lobbyPlayer.ClientId.ToString();
            otherPlayer.NameText.text = lobbyPlayer.PlayerName.ToString();
            otherPlayer.TypeText.text = lobbyPlayer.Type.ToString();
            otherPlayer.ReadyText.text = lobbyPlayer.IsReady ? "Ready" : "Not Ready";

            otherPlayer.transform.SetParent(playerPanel, false);

            otherPlayers.Add(lobbyPlayer.ClientId, otherPlayer);
        }

        public void AddLocalPlayer(LobbyPlayer lobbyPlayer)
        {    
            localPlayer = Instantiate<LocalPlayer>(localPlayerPrefab);

            //    localPlayer.BgImage.color = lobbyPlayer.IsHost ? Color.blue : Color.grey;
            localPlayer.IdText.text = lobbyPlayer.ClientId.ToString();
            localPlayer.NameText.text = lobbyPlayer.PlayerName.ToString();

            localPlayer.ReadyButton.onClick.AddListener(OnClickReady);
            localPlayer.ReadyButton.interactable = true;

            localPlayer.TypeDropdown.value = (int)lobbyPlayer.Type;
            localPlayer.TypeDropdown.onValueChanged.AddListener(OnPlayerTypeChange);
            localPlayer.TypeDropdown.interactable = true;

            localPlayer.transform.SetParent(playerPanel, false);

            if(lobbyPlayer.IsHost)
            {
                startButton.gameObject.SetActive(true);
                startButton.interactable = false;
            }
        }

        public void OnClickStart()
        {
            onStartEvent?.Invoke();
        }

        private void OnPlayerTypeChange(int value)
        {
            onPlayerTypeChangeEvent?.Invoke(value);
        }

        private void OnClickReady()
        {
            localPlayer.ReadyButton.interactable = false;
            localPlayer.TypeDropdown.interactable = false;

            onPlayerReadyEvent?.Invoke();
        }
    }
}
