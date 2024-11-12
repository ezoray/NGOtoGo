using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NGOtoGo.Examples.Lobby
{
    // Contains the UI elements for the local player, this is part of the local player prefab.
    public class LocalPlayer : MonoBehaviour
    {
        [SerializeField] Image bgImage;
        [SerializeField] TextMeshProUGUI idText;
        [SerializeField] TextMeshProUGUI nameText;
        [SerializeField] TMP_Dropdown typeDropdown;
        [SerializeField] Button readyButton;
        [SerializeField] TextMeshProUGUI readyButtonText;

        public Image BgImage { get => bgImage; set => bgImage = value; }
        public TextMeshProUGUI IdText { get => idText; set => idText = value; }
        public TextMeshProUGUI NameText { get => nameText; set => nameText = value; }
        public TMP_Dropdown TypeDropdown { get => typeDropdown; set => typeDropdown = value; }
        public Button ReadyButton { get => readyButton; set => readyButton = value; }
        public TextMeshProUGUI ReadyButtonText { get => readyButtonText; set => readyButtonText = value; }
    }
}
