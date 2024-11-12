using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NGOtoGo.Examples.Lobby
{
    /// <summary>
    /// The scene loaded once the host hits Start in the lobby.
    /// Once all clients have loaded the scene the OnLoadEventCompleted event is triggered and the GamePlayers are spawned.
    /// </summary>
    public class GameSceneController : MonoBehaviour
    {
        [SerializeField] GameDetailSO gameDetail; // brings the player data over from the Lobby scene
        [SerializeField] NetworkObject playerPrefab;

        NetworkManager networkManager;

        private void OnEnable()
        {
            Debug.Log("GameSceneController OnEnable: " + gameDetail);
            networkManager = NetworkManager.Singleton;

            if (networkManager.IsHost)
            {
                networkManager.SceneManager.OnSceneEvent += OnSceneEvent;
                networkManager.SceneManager.OnLoadEventCompleted += OnLoadEventCompleted;
            }
        }

        private void OnSceneEvent(SceneEvent sceneEvent)
        {
            Debug.Log($"GameSceneController OnSceneEvent: clientId: {sceneEvent.ClientId} sceneName: {sceneEvent.SceneName} eventType: {sceneEvent.SceneEventType}");
        }

        private void OnLoadEventCompleted(string sceneName, LoadSceneMode loadSceneMode, List<ulong> clientsCompleted, List<ulong> clientsTimedOut)
        {
            Debug.Log($"GameSceneController OnLoadEventCompleted clientsCompleted: {clientsCompleted.Count} clientsTimedOut: {clientsTimedOut.Count}");

            foreach (var gamePlayer in gameDetail.GamePlayers)
            {
                NetworkObject playerObject = Instantiate<NetworkObject>(playerPrefab);
                playerObject.SpawnAsPlayerObject(gamePlayer.ClientId, true);

                Player player = playerObject.GetComponent<Player>();
                player.PlayerName = gamePlayer.PlayerName.ToString();
                player.PlayerType = gamePlayer.Type;
            }
        }

        private void OnDisable()
        {
            if (networkManager.IsHost)
            {
                networkManager.SceneManager.OnSceneEvent -= OnSceneEvent;
                networkManager.SceneManager.OnLoadEventCompleted -= OnLoadEventCompleted;
            }
        }
    }
}
