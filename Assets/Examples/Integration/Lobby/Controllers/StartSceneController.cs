using UnityEngine;
using UnityEngine.SceneManagement;

namespace NGOtoGo.Examples.Lobby
{
   /// <summary>
   /// To avoid creating duplicates the NetworkManager is instantiated in the first scene which is only loaded once.
   /// </summary>
    public class StartSceneController : MonoBehaviour
    {

        void Start()
        {
            SceneManager.LoadScene("MenuScene", LoadSceneMode.Single);
        }
    }
}
