using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

namespace NGOtoGo.Examples.Lobby
{
    /// <summary>
    /// Represents the player in the Game scene.
    /// Due to the field values being set after spawn they have to be picked up in OnValueChanged which is a bit annoying.
    /// </summary>
    public class Player : NetworkBehaviour
    {
        [SerializeField] NetworkVariable<FixedString32Bytes> playerName = new NetworkVariable<FixedString32Bytes>();
        [SerializeField] NetworkVariable<PlayerType> playerType = new NetworkVariable<PlayerType>();

        private void Awake()
        {
            playerName.OnValueChanged += OnPlayerNameChanged;
            playerType.OnValueChanged += OnPlayerTypeChanged;
        }

        private void OnPlayerTypeChanged(PlayerType previousValue, PlayerType newValue)
        {
            Debug.Log($"Player OnPlayerTypeChanged clientId: {OwnerClientId} newValue: {newValue}");
        }

        private void OnPlayerNameChanged(FixedString32Bytes previousValue, FixedString32Bytes newValue)
        {
            Debug.Log($"Player OnPlayerNameChanged clientId: {OwnerClientId} newValue: {newValue}");
        }

        public override void OnNetworkSpawn()
        {
            Debug.Log($"Player OnNetworkSpawn isLocal: {IsOwner} clientId: {OwnerClientId} name: {playerName.Value} playerType: {playerType.Value}");
        }

        public string PlayerName { get => playerName.Value.ToString(); set => playerName.Value = value; }
        public PlayerType PlayerType { get => playerType.Value; set => playerType.Value = value; }
    }
}
