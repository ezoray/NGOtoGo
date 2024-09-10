using Unity.Netcode;
using UnityEngine;

namespace NGOtoGo.Examples.NetworkVariables.NetworkVariablePermissions
{
    /// <summary>
    /// In-scene network object logging spawn and changed values in network variable fields.
    /// </summary>
    public class InScene : NetworkBehaviour
    {
        // anyone can read but only server/host can write - these are the default permissions and can be omitted
        NetworkVariable<int> anyReadHostWrite_1 = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

        // only owner and server/host can read, only server/host can write - other client reads will return initial value as it's not updated
        NetworkVariable<int> ownerReadHostWrite_2 = new NetworkVariable<int>(0, NetworkVariableReadPermission.Owner, NetworkVariableWritePermission.Server);

        // anyone can read, only the owner can write - writes by non-owners including host result in an error
        NetworkVariable<int> anyReadOwnerWrite_3 = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

        // only owner and server/host can read, only owner can write - writes by non-owners including host result in an error
        NetworkVariable<int> ownerReadOwnerWrite_4 = new NetworkVariable<int>(0, NetworkVariableReadPermission.Owner, NetworkVariableWritePermission.Owner);

        private void Awake()
        {
            anyReadHostWrite_1.OnValueChanged += OnAnyReadHostWriteChanged;
            ownerReadHostWrite_2.OnValueChanged += OnOwnerReadHostWriteChanged;
            anyReadOwnerWrite_3.OnValueChanged += OnAnyReadOwnerWriteChanged;
            ownerReadOwnerWrite_4.OnValueChanged += OnOwnerReadOwnerWriteChanged;
        }

        public override void OnNetworkSpawn()
        {
            Debug.Log("InScene OnNetworkSpawn anyReadHostWrite_1: " + anyReadHostWrite_1.Value);
            Debug.Log("InScene OnNetworkSpawn ownerReadHostWrite_2: " + ownerReadHostWrite_2.Value);
            Debug.Log("InScene OnNetworkSpawn anyReadOwnerWrite_3: " + anyReadOwnerWrite_3.Value);
            Debug.Log("InScene OnNetworkSpawn ownerReadOwnerWrite_4: " + ownerReadOwnerWrite_4.Value);
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

        private void OnOwnerReadOwnerWriteChanged(int previousValue, int newValue)
        {
            Debug.Log($"InScene OnValueChanged ownerReadOwnerWrite_4: oldValue: {previousValue} newValue: {newValue}");
        }

        private void OnAnyReadOwnerWriteChanged(int previousValue, int newValue)
        {
            Debug.Log($"InScene OnValueChanged anyReadOwnerWrite_3: oldValue: {previousValue} newValue: {newValue}");
        }

        private void OnOwnerReadHostWriteChanged(int previousValue, int newValue)
        {
            Debug.Log($"InScene OnValueChanged ownerReadHostWrite_2: oldValue: {previousValue} newValue: {newValue}");
        }

        private void OnAnyReadHostWriteChanged(int previousValue, int newValue)
        {
            Debug.Log($"InScene OnValueChanged anyReadHostWrite_1: oldValue: {previousValue} newValue: {newValue}");
        }

        public override void OnNetworkDespawn()
        {
            anyReadHostWrite_1.OnValueChanged -= OnAnyReadHostWriteChanged;
            ownerReadHostWrite_2.OnValueChanged -= OnOwnerReadHostWriteChanged;
            anyReadOwnerWrite_3.OnValueChanged -= OnAnyReadOwnerWriteChanged;
            ownerReadOwnerWrite_4.OnValueChanged -= OnOwnerReadOwnerWriteChanged;
        }

        public int AnyReadHostWrite_1 { get => anyReadHostWrite_1.Value; set => anyReadHostWrite_1.Value = value; }
        public int OwnerReadHostWrite_2 { get => ownerReadHostWrite_2.Value; set => ownerReadHostWrite_2.Value = value; }
        public int AnyReadOwnerWrite_3 { get => anyReadOwnerWrite_3.Value; set => anyReadOwnerWrite_3.Value = value; }
        public int OwnerReadOwnerWrite_4 { get => ownerReadOwnerWrite_4.Value; set => ownerReadOwnerWrite_4.Value = value; }
    }
}
