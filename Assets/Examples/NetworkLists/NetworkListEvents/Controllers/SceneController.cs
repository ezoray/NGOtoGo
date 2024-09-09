using TMPro;
using Unity.Netcode;
using UnityEngine;

namespace NGOtoGo.Examples.NetworkLists.NetworkListEvents
{
    /// <summary>
    /// Modifies a network list via the UI to demonstrate the triggering of events.
    /// Network list permissions can be changed on the InSceneObject in the Unity Editor.
    /// </summary>
    public class SceneController : MonoBehaviour
    {
        [SerializeField] InScene inSceneObject;
        [SerializeField] TMP_InputField valueField;
        [SerializeField] TMP_InputField indexField;

        NetworkManager networkManager;
        int inputValue = 10;
        int inputIndex = 0;

        void Start()
        {
            Application.targetFrameRate = 15;
            networkManager = NetworkManager.Singleton;

            valueField.text = inputValue.ToString();
            valueField.onEndEdit.AddListener(OnValueChange);

            indexField.text = inputIndex.ToString();
            indexField.onEndEdit.AddListener(OnIndexChange);

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

        private void OnIndexChange(string index)
        {
            if (!int.TryParse(index, out inputIndex))
            {
                valueField.text = inputIndex.ToString();
            }
        }

        private void OnValueChange(string value)
        {
            if(!int.TryParse(value, out inputValue))
            {
                valueField.text = inputValue.ToString();
            }
        }

        public void OnClickSwitchOwner()
        {
            // only host can change ownership, doing so on client will result in an error

            foreach (var clientId in networkManager.ConnectedClientsIds)
            {
                if (inSceneObject.OwnerClientId != clientId)
                {
                    inSceneObject.NetworkObject.ChangeOwnership(clientId);
                    break;
                }
            }
        }

        public void OnClickClear()
        {
            inSceneObject.NetworkList.Clear();
        }

        public void OnClickRemoveAtIndex()
        {
            if (inputIndex >= 0 && inputIndex < inSceneObject.NetworkList.Count)
            {
                inSceneObject.NetworkList.RemoveAt(inputIndex);
            }
        }

        public void OnClickRemoveValue()
        {
            bool isRemoved = inSceneObject.NetworkList.Remove(inputValue);
        }

        public void OnClickChangeValue()
        {
            if (inputIndex >= 0 && inputIndex < inSceneObject.NetworkList.Count)
            {
                inSceneObject.NetworkList[inputIndex] = inputValue;
            }
        }

        public void OnClickInsertValue()
        {
            if (inputIndex >= 0 && inputIndex <= inSceneObject.NetworkList.Count)
            {
                inSceneObject.NetworkList.Insert(inputIndex, inputValue);
            }
        }

        public void OnClickAddValue()
        {
            inSceneObject.NetworkList.Add(inputValue);
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
            valueField.onEndEdit.RemoveAllListeners();
            indexField.onEndEdit.RemoveAllListeners();

            networkManager.OnClientConnectedCallback -= OnClientConnected;
            networkManager.OnClientDisconnectCallback -= OnClientDisconnect;
        }
    }
}
