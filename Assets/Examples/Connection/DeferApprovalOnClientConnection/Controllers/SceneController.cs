using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;


namespace NGOtoGo.Examples.Connection.DeferApprovalOnClientConnection
{
    /// <summary>
    /// Connecting clients have their approval set to Pending in ConnectionApproval.
    /// The host decides whether to approve or deny the connection in the UI and sets the response accordingly.
    /// </summary>
    public class SceneController : MonoBehaviour
    {
        [SerializeField] GameObject canvas;
        [SerializeField] TextMeshProUGUI connectedText;
        [SerializeField] Button approveButton;
        [SerializeField] Button denyButton;

        NetworkManager networkManager;
        NetworkManager.ConnectionApprovalResponse pendingResponse;

        void Start()
        {
            Application.targetFrameRate = 15;
            networkManager = NetworkManager.Singleton;

            networkManager.OnClientConnectedCallback += OnClientConnected;
            networkManager.OnClientDisconnectCallback += OnClientDisconnect;

            if (!ParrelSync.ClonesManager.IsClone())
            {
                ResetGui();

                networkManager.ConnectionApprovalCallback += OnConnectionApproval;
                networkManager.StartHost();
            }
            else
            {
                canvas.SetActive(false);

                networkManager.StartClient();
                Debug.Log("SceneController Start connecting to host...");
            }
        }

        private void OnConnectionApproval(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response)
        {
            Debug.Log($"SceneController OnConnectionApproval clientId: {request.ClientNetworkId}");

            if (request.ClientNetworkId == networkManager.LocalClientId)
            {
                response.Approved = true;
            }
            else
            {
                response.Pending = true;
                pendingResponse = response;

                EnableClientApprovalCheck(request.ClientNetworkId);
            }
        }

        private void EnableClientApprovalCheck(ulong clientNetworkId)
        {
            connectedText.text = $"Client {clientNetworkId} connected!";
            approveButton.interactable = true;
            denyButton.interactable = true;
        }

        public void OnClickApproveConnection()
        {
            Debug.Log("SceneController OnClickApproveConnection");

            pendingResponse.Pending = false;
            pendingResponse.Approved = true;

            ResetGui();
        }

        public void OnClickDenyConnection()
        {
            Debug.Log("SceneController OnClickDenyConnection");

            pendingResponse.Pending = false;
            pendingResponse.Approved = false;
            pendingResponse.Reason = "Connection denied.";

            ResetGui();
        }

        private void ResetGui()
        {
            connectedText.text = string.Empty;
            approveButton.interactable = false;
            denyButton.interactable = false;
        }

        private void OnClientConnected(ulong clientId)
        {
            Debug.Log($"SceneController OnClientConnected clientId: {clientId}");
        }

        private void OnClientDisconnect(ulong clientId)
        {
            Debug.Log($"SceneController OnClientDisconnect clientId: {clientId} {networkManager.DisconnectReason}");
        }

        private void OnDestroy()
        {
            networkManager.OnClientConnectedCallback -= OnClientConnected;
            networkManager.OnClientDisconnectCallback -= OnClientDisconnect;

            if (networkManager.IsHost)
            {
                networkManager.ConnectionApprovalCallback -= OnConnectionApproval;
            }
        }
    }
}
