using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

namespace NGOtoGo.Examples.NetworkLists.NetworkListEvents
{
    /// <summary>
    /// In-scene network object containing the network list.
    /// Subscribes to the list's OnListChanged and logs the network list change events.
    /// Read/write permissions can changed in the inspector.
    /// </summary>
    public class InScene : NetworkBehaviour
    {
        [SerializeField] NetworkVariableReadPermission listReadPermission = NetworkVariableReadPermission.Everyone;
        [SerializeField] NetworkVariableWritePermission listWritePermission = NetworkVariableWritePermission.Owner;
        [SerializeField] TextMeshProUGUI countText;

        NetworkList<int> networkList;

        private void Awake()
        {
            networkList = new NetworkList<int>(new List<int>(), listReadPermission, listWritePermission);
            networkList.OnListChanged += OnListChanged;
        }

        public override void OnNetworkSpawn()
        {
            Debug.Log("InScene OnNetworkSpawn list count: " + networkList.Count);

            countText.text = networkList.Count.ToString();

            for (int i = 0; i < networkList.Count; i++)
            {
                Debug.Log($"List index: {i} value: {networkList[i]}", this);
            }
        }

        private void Start()
        {
            Debug.Log("InScene Start");
        }

        public override void OnLostOwnership()
        {
            Debug.Log("InScene OnLostOwnership:" + OwnerClientId);
        }

        public override void OnGainedOwnership()
        {
            Debug.Log("InScene OnGainedOwnership:" + OwnerClientId);
        }

        private void OnListChanged(NetworkListEvent<int> changeEvent)
        {
       //     Debug.Log($"InScene OnListChanged type: {changeEvent.Type} index: {changeEvent.Index} oldValue: {changeEvent.PreviousValue} newValue: {changeEvent.Value}");

            switch (changeEvent.Type)
            {
                case NetworkListEvent<int>.EventType.Add:
                    Debug.Log($"InScene OnListChanged type: {changeEvent.Type} value: {changeEvent.Value}");
                    break;

                case NetworkListEvent<int>.EventType.Insert:
                    Debug.Log($"InScene OnListChanged type: {changeEvent.Type} index: {changeEvent.Index} value: {changeEvent.Value}");
                    break;

                case NetworkListEvent<int>.EventType.Remove:
                    Debug.Log($"InScene OnListChanged type: {changeEvent.Type} value: {changeEvent.Value}");
                    break;

                case NetworkListEvent<int>.EventType.RemoveAt:
                    Debug.Log($"InScene OnListChanged type: {changeEvent.Type} index: {changeEvent.Index} value: {changeEvent.Value}");
                    break;

                case NetworkListEvent<int>.EventType.Value:
                    Debug.Log($"InScene OnListChanged type: {changeEvent.Type} index: {changeEvent.Index} oldValue: {changeEvent.PreviousValue} newValue: {changeEvent.Value}");
                    break;

                case NetworkListEvent<int>.EventType.Clear:
                    Debug.Log($"InScene OnListChanged type: {changeEvent.Type}");
                    break;

                case NetworkListEvent<int>.EventType.Full:
                    // no idea how this is triggered
                    break;
                default:
                    break;
            }

            countText.text = networkList.Count.ToString();

            for (int i = 0; i < networkList.Count; i++)
            {
                Debug.Log($"List index: {i} value: {networkList[i]}", this);
            }
        }

        public override void OnNetworkDespawn()
        {
            networkList.OnListChanged -= OnListChanged;
        }

        public NetworkList<int> NetworkList { get => networkList; set => networkList = value; }
    }
}
