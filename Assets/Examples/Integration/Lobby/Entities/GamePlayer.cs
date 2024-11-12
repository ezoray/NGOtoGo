using System;
using System.Text;
using NGOtoGo.Examples.Lobby;
using Unity.Collections;

namespace NGOtoGo.Examples
{
    /// <summary>
    /// Represents the player data from the lobby that is used in the Game scene when spawning the players.
    /// </summary>
    [Serializable]
    public struct GamePlayer
    {
        public ulong ClientId;
        public FixedString32Bytes PlayerName;
        public PlayerType Type;

        public GamePlayer(ulong clientId, FixedString32Bytes playerName, PlayerType type)
        {
            ClientId = clientId;
            PlayerName = playerName;
            Type = type;
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder("GamePlayer");
            stringBuilder.Append(" ClientId ").Append(ClientId);
            stringBuilder.Append(" PlayerName ").Append(PlayerName);
            stringBuilder.Append(" Type ").Append(Type);

            return stringBuilder.ToString();
        }
    }
}
