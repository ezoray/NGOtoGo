using TMPro;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

namespace NGOtoGo.Examples.Messaging.SendAndReceiveUnnamedMessages
{
    /// <summary>
    /// Send and receive unnamed messages.
    /// On connection subscribes to receive unnamed messages.
    /// Messages to send are input and received messages output on the UI.
    /// </summary>
    public class SceneController : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI receivedText;
        [SerializeField] TMP_InputField inputText;

        NetworkManager networkManager;
        CustomMessagingManager messagingManager;

        private void Awake()
        {
            inputText.onEndEdit.AddListener(OnInputText);

            EnableGui(false);
        }

        void Start()
        {
            Application.targetFrameRate = 15;
            networkManager = NetworkManager.Singleton;

            networkManager.OnClientConnectedCallback += OnClientConnected;
            networkManager.OnClientDisconnectCallback += OnClientDisconnect;


            if (!ParrelSync.ClonesManager.IsClone())
            {
                networkManager.StartHost();
            }
            else
            {
                networkManager.StartClient();
            }
        }

        private void OnReceiveMessage(ulong clientId, FastBufferReader reader)
        {
            if (networkManager.IsHost && clientId == NetworkManager.ServerClientId)
                return;

            reader.ReadValueSafe(out string message);

            Debug.Log($"SceneController OnReceiveMessage: {message}");

            receivedText.text = message;
        }

        public void SendUnnamedMessage(string message)
        {
            Debug.Log($"SceneController SendUnnamedMessage length: {message.Length} {message}");

            // arbitrary value added to length as overhead, this needs to be considerably larger for longer messages
            var writer = new FastBufferWriter(message.Length + 256, Allocator.Temp);

            using (writer)
            {             
                writer.WriteValueSafe(message);

                Debug.Log($"SceneController SendUnnamedMessage message writer length: {writer.Length}");

                if (networkManager.IsHost)
                {
                    messagingManager.SendUnnamedMessageToAll(writer);
                }
                else
                {
                    // set as NetworkDelivery.ReliableFragmentedSequenced for long messages
                    messagingManager.SendUnnamedMessage(NetworkManager.ServerClientId, writer, NetworkDelivery.ReliableSequenced);
                }
            }
        }

        private void OnInputText(string message)
        {
            if (!string.IsNullOrEmpty(message))
            {
                SendUnnamedMessage(message);
                inputText.text = string.Empty;
            }
        }

        private void OnClientConnected(ulong clientId)
        {
            Debug.Log($"SceneController OnClientConnected: {clientId}");

            if (networkManager.IsHost && clientId != NetworkManager.ServerClientId)
                return;

            messagingManager = networkManager.CustomMessagingManager;
            messagingManager.OnUnnamedMessage += OnReceiveMessage;
            EnableGui(true);
        }

        private void OnClientDisconnect(ulong clientId)
        {
            Debug.Log($"SceneController OnClientDisconnect: {clientId}");

            if (networkManager.IsHost && clientId != NetworkManager.ServerClientId)
                return;

            messagingManager.OnUnnamedMessage -= OnReceiveMessage;
        }

        private void EnableGui(bool enable)
        {
            if (enable)
            {
                receivedText.text = string.Empty;
            }

            inputText.interactable = enable;
        }

        private void OnDestroy()
        {
            inputText.onEndEdit.RemoveListener(OnInputText);

            networkManager.OnClientConnectedCallback -= OnClientConnected;
            networkManager.OnClientDisconnectCallback -= OnClientDisconnect;
        }
    }
}
