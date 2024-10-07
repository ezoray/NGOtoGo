using System;
using Unity.Netcode;
using UnityEngine;

namespace NGOtoGo.Examples.Integration.PerlinTilemap
{
    /// <summary>
    /// Example of drawing a Perlin based tilemap on host and client.
    /// Parameter values can be changed in the inspector which triggers OnValidate.
    /// This code is not fast and created purely as a short example.
    /// </summary>
    public class SceneController : MonoBehaviour
    {
        [SerializeField] WorldManager worldManager;

        [SerializeField, Range(0.1f, 100f)] float scale = 12f;
        [SerializeField, Range(1, 5)] int octaves = 5;
        [SerializeField, Range(0.1f, 1f)] float persistence = 0.5f;
        [SerializeField, Range(0.1f, 10f)] float lacunarity = 1f;
        [SerializeField] Vector2Int offset = Vector2Int.zero;

        [SerializeField, Range(1, 400)] int worldWidth = 100;
        [SerializeField, Range(1, 400)] int worldHeight = 100;
        [SerializeField, Range(1, 100)] int seed = 50;

        NetworkManager networkManager;
        bool isServerStarted;

        private void Start()
        {
            Application.targetFrameRate = 15; // arbitrary framerate limit

            networkManager = NetworkManager.Singleton;

            if (!ParrelSync.ClonesManager.IsClone())
            {
                networkManager.OnServerStarted += OnServerStarted;
                networkManager.OnServerStopped += OnServerStopped;
                networkManager.StartHost();
            }
            else
            {
                networkManager.StartClient();
            }
        }
  
        private void OnValidate()
        {
            Debug.Log("SceneController OnValidate");

            if (isServerStarted)
            {
                worldManager.UpdateWorld(scale, octaves, persistence, lacunarity, offset, worldWidth, worldHeight, seed);
            }
        }

        private void OnServerStopped(bool isHost)
        {
            Debug.Log("SceneController OnServerStopped");
            isServerStarted = false;
        }

        private void OnServerStarted()
        {
            Debug.Log("SceneController OnServerStarted");
            isServerStarted = true;

            worldManager.UpdateWorld(scale, octaves, persistence, lacunarity, offset, worldWidth, worldHeight, seed);
        }
    }
}
