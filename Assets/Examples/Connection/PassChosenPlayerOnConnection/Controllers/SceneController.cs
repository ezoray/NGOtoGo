using System;
using Unity.Netcode;
using UnityEngine;

namespace NGOtoGo.Examples.Connection.PassChosenPlayerOnConnection
{
    /// <summary>
    /// The host/client pass the hash of the player object they wish to be their Player on connection.
    /// ConnectionApproval matches the hash and approves the connection.
    /// Note host connections are always approved.
    /// In the example there is always a match but if there wasn't the client would be disconnected with reason.
    /// </summary>
    public class SceneController : MonoBehaviour
    {
        [SerializeField] NetworkObject malePlayerPrefab;
        [SerializeField] NetworkObject femalePlayerPrefab;
        NetworkManager networkManager;

        void Start()
        {
            Application.targetFrameRate = 15;

            networkManager = NetworkManager.Singleton;

            networkManager.OnClientDisconnectCallback += OnClientDisconnect;

            if (!ParrelSync.ClonesManager.IsClone())
            {
                networkManager.ConnectionApprovalCallback += OnConnectionApproval;
                networkManager.NetworkConfig.ConnectionData = BitConverter.GetBytes(malePlayerPrefab.PrefabIdHash);
                networkManager.StartHost();
            }
            else
            {
                networkManager.NetworkConfig.ConnectionData = BitConverter.GetBytes(femalePlayerPrefab.PrefabIdHash);
                networkManager.StartClient();
            }
        }

        private void OnConnectionApproval(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response)
        {
            uint globalObjectIdHash = BitConverter.ToUInt32(request.Payload, 0);

            Debug.Log($"SceneController OnConnectionApproval clientId: {request.ClientNetworkId} payload hash: {globalObjectIdHash}");

            NetworkPrefabs prefabs = networkManager.NetworkConfig.Prefabs;

            foreach (var prefab in prefabs.Prefabs)
            {
                if (globalObjectIdHash == prefab.SourcePrefabGlobalObjectIdHash)
                {
                    Debug.Log("SceneController OnConnectionApproval Found prefab for hash, connection approved.");

                    response.PlayerPrefabHash = globalObjectIdHash;
                    response.CreatePlayerObject = true;
                    response.Approved = true;
                    return;
                }
            }

            response.Reason = "No prefab for hash, connection denied.";
            response.Approved = false;
        }

        private void OnClientDisconnect(ulong clientId)
        {
            Debug.Log($"SceneController OnClientDisconnect clientId: {clientId} {networkManager.DisconnectReason}");
        }

        private void OnDestroy()
        {
            networkManager.OnClientDisconnectCallback -= OnClientDisconnect;

            if(networkManager.IsHost)
            {
                networkManager.ConnectionApprovalCallback -= OnConnectionApproval;
            }
        }
    }
}
