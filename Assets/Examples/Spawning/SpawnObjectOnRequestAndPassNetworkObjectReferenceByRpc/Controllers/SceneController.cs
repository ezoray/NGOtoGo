using Unity.Netcode;
using UnityEngine;

namespace NGOtoGo.Examples.Spawning.SpawnObjectOnRequestAndPassNetworkObjectReferenceByRpc
{
    /// <summary>
    /// Clicking the Spawn button on host or client will send an RPC to the host to spawn an object and its
    /// NetworkObjectReference will be RPC'ed to everyone connected.
    /// </summary>
    public class SceneController : MonoBehaviour
    {
        [SerializeField] InSceneObject inSceneObject;
        NetworkManager networkManager;

        private void Start()
        {
            Application.targetFrameRate = 15; // arbitrary framerate limit

            networkManager = NetworkManager.Singleton;

            if (!ParrelSync.ClonesManager.IsClone())
            {             
                networkManager.StartHost();
            }
            else
            {               
                networkManager.StartClient();
            }
        }

        public void OnClickSpawnObject()
        {
            if(networkManager.IsConnectedClient)
            {
                inSceneObject.SpawnObjectRpc();
            }
        }

        private void OnDestroy()
        {
            networkManager.Shutdown();
        }
    }
}
