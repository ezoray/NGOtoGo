using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NGOtoGo.Examples.Events.SubscribeToNetworkEvents
{
    /// <summary>
    ///  Subscribes to network events to give demonstrate when and where called
    /// </summary>
    public class SceneController : MonoBehaviour
    {
        [SerializeField] NetworkObject inSceneObject;
        NetworkManager networkManager;

        void Start()
        {
            Application.targetFrameRate = 15;
            networkManager = NetworkManager.Singleton;

            // called on host
            networkManager.OnServerStarted += OnServerStarted;
            networkManager.OnServerStopped += OnServerStopped;

            // called on host and client
            networkManager.OnClientStarted += OnClientStarted;
            networkManager.OnClientStopped += OnClientStopped;

            // this can be used in place of below connection events
            networkManager.OnConnectionEvent += OnConnectionEvent;

            networkManager.OnClientConnectedCallback += OnClientConnectedCallback;
            networkManager.OnClientDisconnectCallback += OnClientDisconnectCallback;

            // uncomment this for the low level transport events
            //networkManager.NetworkConfig.NetworkTransport.OnTransportEvent += OnTransportEvent;      

            if (!ParrelSync.ClonesManager.IsClone())
            {
                // only host need subscribe to this
                networkManager.ConnectionApprovalCallback += ConnectionApproval;

                networkManager.StartHost();
            }
            else
            {
                networkManager.StartClient();
            }

            // called here as SceneManager is instantiated in StartHost/Client 
            networkManager.SceneManager.OnSceneEvent += OnSceneEvent;
        }

        private void OnSceneEvent(SceneEvent sceneEvent)
        {
            Debug.Log($"OnSceneEvent: clientId: {sceneEvent.ClientId} sceneName: {sceneEvent.SceneName} eventType: {sceneEvent.SceneEventType}");
        }

        private void OnTransportEvent(NetworkEvent eventType, ulong clientId, ArraySegment<byte> payload, float receiveTime)
        {
            Debug.Log($"OnTransportEvent eventType: {eventType} clientId: {clientId} payload: {payload.Count} receiveTime: {receiveTime}");
        }

        private void ConnectionApproval(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response)
        {
            Debug.Log($"ConnectionApproval clientId: {request.ClientNetworkId}");

            response.CreatePlayerObject = true;
            response.Approved = true;
        }

        private void OnConnectionEvent(NetworkManager networkManager, ConnectionEventData eventData)
        {
            Debug.Log($"OnConnectionEvent: clientId: {eventData.ClientId} eventType: {eventData.EventType}");

            switch (eventData.EventType)
            {
                case ConnectionEvent.ClientConnected:
                    break;

                // on client connection switch ownership of inScene object to that client to demonstate ownership change events
                case ConnectionEvent.PeerConnected:
                    inSceneObject.ChangeOwnership(eventData.ClientId);
                    break;

                case ConnectionEvent.ClientDisconnected:
                    break;
                case ConnectionEvent.PeerDisconnected:
                    break;
                default:
                    break;
            }
        }

        private void OnClientDisconnectCallback(ulong clientId)
        {
            Debug.Log($"OnClientDisconnectCallback clientId: {clientId}");
        }

        private void OnClientConnectedCallback(ulong clientId)
        {
            Debug.Log($"OnClientConnectedCallback clientId: {clientId}");
        }

        private void OnClientStopped(bool isHost)
        {
            Debug.Log($"OnClientStopped isHost: {isHost}");
        }

        private void OnClientStarted()
        {
            Debug.Log("OnClientStarted");           
        }

        private void OnServerStopped(bool isHost)
        {
            Debug.Log($"OnServerStopped isHost {isHost}");
        }

        private void OnServerStarted()
        {
            Debug.Log("OnServerStarted");

            // don't load as Single as this SceneController will be destroyed and the event callbacks lost
            networkManager.SceneManager.LoadScene("ConnectedScene", LoadSceneMode.Additive);
        }

        private void OnDestroy()
        {
            if(networkManager.IsHost)
            {
                networkManager.ConnectionApprovalCallback -= ConnectionApproval;
            }

            networkManager.OnServerStarted -= OnServerStarted;
            networkManager.OnServerStopped -= OnServerStopped;
            networkManager.OnClientStarted -= OnClientStarted;
            networkManager.OnClientStopped -= OnClientStopped;
            networkManager.OnClientConnectedCallback -= OnClientConnectedCallback;
            networkManager.OnClientDisconnectCallback -= OnClientDisconnectCallback;
            networkManager.OnConnectionEvent -= OnConnectionEvent;
        }
    }
}
