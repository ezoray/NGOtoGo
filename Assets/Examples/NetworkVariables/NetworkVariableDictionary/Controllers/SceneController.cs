using System;
using TMPro;
using Unity.Netcode;
using UnityEngine;

namespace NGOtoGo.Examples.NetworkVariables.NetworkVariableDictionary
{
    /// <summary>
    /// Demonstrates how to setup and use a NetworkVariable dictionary.
    /// The dictionary is modified by user input which triggers a change event, the changes in the dictionary are then logged.
    /// </summary>
    public class SceneController : MonoBehaviour
    {
        [SerializeField] InScene inSceneObject;
        [SerializeField] TextMeshProUGUI valueText;
        [SerializeField] TextMeshProUGUI indexText;

        NetworkManager networkManager;
        int inputValue = 10;
        int inputIndex = 0;
        private bool isConnected;

        void Start()
        {
            Application.targetFrameRate = 15;
            networkManager = NetworkManager.Singleton;

            valueText.text = inputValue.ToString();
            indexText.text = inputIndex.ToString();

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

        public void OnClickDictionaryUpdate(string updateType)
        {
            if (Enum.TryParse(updateType, out UpdateType parsedUpdateType))
            {
                inSceneObject.DictionaryUpdate(parsedUpdateType, inputIndex, inputValue);
            }
        }

        public void OnClickIncreaseValue()
        {
            inputValue++;
            valueText.text = inputValue.ToString();
        }

        public void OnClickDecreaseValue()
        {
            inputValue--;
            valueText.text = inputValue.ToString();
        }

        public void OnClickIncreaseIndex()
        {
            inputIndex++;
            indexText.text = inputIndex.ToString();
        }

        public void OnClickDecreaseIndex()
        {
            inputIndex--;
            indexText.text = inputIndex.ToString();
        }

        public void OnClickSwitchOwner()
        {
            foreach (var clientId in networkManager.ConnectedClientsIds)
            {
                if (inSceneObject.OwnerClientId != clientId)
                {
                    inSceneObject.NetworkObject.ChangeOwnership(clientId);
                    break;
                }
            }
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