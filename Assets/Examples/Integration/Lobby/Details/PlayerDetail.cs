using System;
using System.Text;

namespace NGOtoGo.Examples.Lobby
{
    /// <summary>
    /// Contains the player's data from the Menu scene
    /// </summary>
    [Serializable]
    public struct PlayerDetail
    {
        public string Name;
        public PlayerType Type;

        public PlayerDetail(string name, PlayerType type)
        {
            Name = name;
            Type = type;
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder("PlayerDetail");
            stringBuilder.Append(" Name ").Append(Name);
            stringBuilder.Append(" Type ").Append(Type);

            return stringBuilder.ToString();
        }
    }
}
