using System;
using System.Text;
using Unity.Collections;
using Unity.Netcode;

namespace NGOtoGo.Examples.Lobby
{
    /// <summary>
    /// Contains the data for each player in the Lobby
    /// This is serializable as it's passed to each client via the Lobby's network list.
    /// Passing all the player data around each time there's a field change isn't very efficient and can be improved, either
    /// flag and serialise the field that changed or use a different approach.
    /// </summary>
    public struct LobbyPlayer : INetworkSerializeByMemcpy, IEquatable<LobbyPlayer>
    {
        public ulong ClientId;
        public FixedString32Bytes PlayerName;
        public PlayerType Type;
        public bool IsHost;
        public bool IsReady;

        public LobbyPlayer(ulong clientId, string playerName, PlayerType playerType, bool isHost)
        {
            ClientId = clientId;
            PlayerName = playerName;
            Type = playerType;
            IsHost = isHost;
            IsReady = false;
        }

        public bool Equals(LobbyPlayer other)
        {
            return other.ClientId == ClientId;
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder("LobbyPlayer");
            stringBuilder.Append(" ClientId ").Append(ClientId);
            stringBuilder.Append(" PlayerName ").Append(PlayerName);
            stringBuilder.Append(" Type ").Append(Type);
            stringBuilder.Append(" IsHost ").Append(IsHost);
            stringBuilder.Append(" IsReady ").Append(IsReady);

            return stringBuilder.ToString();
        }
    }
}
