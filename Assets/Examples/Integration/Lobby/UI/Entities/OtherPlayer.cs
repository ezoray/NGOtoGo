using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NGOtoGo.Examples.Lobby
{
    // Contains the UI elements for the other non-local players, this is part of the other player prefab.
    public class OtherPlayer : MonoBehaviour
    {
        [SerializeField] Image bgImage;
        [SerializeField] TextMeshProUGUI idText;
        [SerializeField] TextMeshProUGUI nameText;
        [SerializeField] TextMeshProUGUI typeText;
        [SerializeField] TextMeshProUGUI readyText;

        public Image BgImage { get => bgImage; set => bgImage = value; }
        public TextMeshProUGUI IdText { get => idText; set => idText = value; }
        public TextMeshProUGUI NameText { get => nameText; set => nameText = value; }
        public TextMeshProUGUI TypeText { get => typeText; set => typeText = value; }
        public TextMeshProUGUI ReadyText { get => readyText; set => readyText = value; }
    }
}
