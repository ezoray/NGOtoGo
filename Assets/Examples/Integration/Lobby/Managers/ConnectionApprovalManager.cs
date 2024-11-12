using System;
using System.Collections.Generic;
using System.Text;
using Unity.Netcode;
using UnityEngine;

namespace NGOtoGo.Examples.Lobby
{
    /// <summary>
    /// Class to handle the approval of the client.
    /// Approval is used as a convenient way for the host to receive the player name and type data upon connecting.
    /// For this example the class is a component of the NetworkManager game object as a simple way to persist it over scene changes.
    /// Approved client data is stored so that it can be retrieved later when the OnClientConnected callback is triggered.
    /// </summary>
    public class ConnectionApprovalManager : MonoBehaviour
    {
        [SerializeField] NetworkManager networkManager;

        Dictionary<ulong, PlayerDetail> joinedPlayerDetails;

        private void OnEnable()
        {
            joinedPlayerDetails = new Dictionary<ulong, PlayerDetail>();

            networkManager.ConnectionApprovalCallback += OnConnectionApproval;            
        }

        private void OnConnectionApproval(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response)
        {
            Debug.Log($"ConnectionHandler OnConnectionApproval clientId: {request.ClientNetworkId}");                   

            string playerJson = Encoding.UTF8.GetString(request.Payload);
            PlayerDetail playerDetail = JsonUtility.FromJson<PlayerDetail>(playerJson);
        
            Debug.Log("ConnectionHandler OnConnectionApproval " + playerDetail);

            joinedPlayerDetails.Add(request.ClientNetworkId, playerDetail);

            response.Approved = true;
        }

        public bool TryGetJoinedPlayer(ulong clientId, out PlayerDetail joinedPlayer)
        {
            if(joinedPlayerDetails.TryGetValue(clientId, out joinedPlayer))
            {
                joinedPlayerDetails.Remove(clientId);
                return true;
            }

            return false;
        }

        private void OnDisable()
        {
            networkManager.ConnectionApprovalCallback -= OnConnectionApproval;
        }
    }
}
