using System.Text;
using ParrelSync;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace NGOtoGo.Examples.Lobby
{
    /// <summary>
    /// This scene allows the player to enter name and choose player type, and to start the host/client.
    /// This player data is passed on connection.
    /// Having player type here isn't necessary as the player can still choose the type in the lobby, normally it's one or the other.
    /// </summary>
    public class MenuSceneController : MonoBehaviour
    {
        [SerializeField] TMP_InputField nameField;
        [SerializeField] TMP_Dropdown typeField;
        [SerializeField] Button startButton;
        NetworkManager networkManager;

        void Start()
        {
            Application.targetFrameRate = 15;
            networkManager = NetworkManager.Singleton;
                       
            if (!ClonesManager.IsClone())
            { 
                nameField.text = startButton.GetComponentInChildren<TextMeshProUGUI>().text = "Host";
            }
            else
            {
                nameField.text = ClonesManager.GetArgument();
                startButton.GetComponentInChildren<TextMeshProUGUI>().text = "Connect";
            }
        }

        private void OnHostConnected(ulong clientId)
        {
            Debug.Log("MenuSceneController OnHostConnected");

            networkManager.SceneManager.LoadScene("LobbyScene", LoadSceneMode.Single);
        }

        public void OnClickStart()
        {
            PlayerDetail playerDetail = new PlayerDetail(nameField.text, (PlayerType)typeField.value);
            string playerJson = JsonUtility.ToJson(playerDetail);
            networkManager.NetworkConfig.ConnectionData = Encoding.UTF8.GetBytes(playerJson);

            if (!ParrelSync.ClonesManager.IsClone())
            {                
                networkManager.OnClientConnectedCallback += OnHostConnected;
                networkManager.StartHost();
            }
            else
            {
                networkManager.StartClient();
            }            
        }

        private void OnDestroy()
        {
            if (networkManager.IsHost)
            {                
                networkManager.OnClientConnectedCallback -= OnHostConnected;
            }
        }
    }
}
