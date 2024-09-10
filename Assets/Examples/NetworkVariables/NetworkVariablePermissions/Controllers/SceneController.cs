using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace NGOtoGo.Examples.NetworkVariables.NetworkVariablePermissions
{
    /// <summary>
    /// Demonstrates how permissions affect the behaviour of network variables in a network object.
    /// On click attempts to increment the network variable values.
    /// On click switches ownership of object to reflect changes in network variable behaviour based on ownership.
    /// </summary>
    public class SceneController : MonoBehaviour
    {
        [SerializeField] InScene inSceneObject;
        NetworkManager networkManager;

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

        public void OnClickSwitchOwner()
        {
            // only host can change ownership, doing so on client will result in an error

            foreach(var clientId in networkManager.ConnectedClientsIds)
            {
                if(inSceneObject.OwnerClientId != clientId)
                {
                    inSceneObject.NetworkObject.ChangeOwnership(clientId);
                    break;
                }
            }            
        }

        public void OnClickIncrementValues()
        {
            inSceneObject.AnyReadHostWrite_1++;
            inSceneObject.OwnerReadHostWrite_2++;
            inSceneObject.AnyReadOwnerWrite_3++;
            inSceneObject.OwnerReadOwnerWrite_4++;
        }

        private void OnClientConnected(ulong clientId)
        {
            Debug.Log($"SceneController OnClientConnected: {clientId}");
        }

        private void OnClientDisconnect(ulong clientId)
        {
            Debug.Log($"SceneController OnClientDisconnect: {clientId}");
        }

        private void OnDestroy()
        {
            networkManager.OnClientConnectedCallback -= OnClientConnected;
            networkManager.OnClientDisconnectCallback -= OnClientDisconnect;
        }
    }
}
