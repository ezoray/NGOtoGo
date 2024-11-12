using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace NGOtoGo.Examples.Lobby
{
    /// <summary>
    /// Used to pass the player data from the Lobby to the Game scene.
    /// </summary>
    [CreateAssetMenu(fileName = "GameDetail", menuName = "Scriptables/GameDetail")]
    public class GameDetailSO : ScriptableObject
    {
        public List<GamePlayer> GamePlayers;

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder("GameDetailSO");

            foreach (var player in GamePlayers)
            {
                stringBuilder.Append(player).Append("\n");
            }

            return stringBuilder.ToString();
        }
    }
}
