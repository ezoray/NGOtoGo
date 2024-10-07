using Unity.Netcode;
using UnityEngine;

namespace NGOtoGo.Examples.Integration.PerlinTilemap
{
    /// <summary>
    /// Handles the creation and drawing of a Perlin based tilemap.
    /// The details for drawing are shared via a NetworkVariable so host and client generate their own identical tilemaps.
    /// </summary>
    public class WorldManager : NetworkBehaviour
    {
        NetworkVariable<WorldDetail> worldDetail = new NetworkVariable<WorldDetail>();

        [SerializeField] PerlinNoiseGenerator noiseGenerator;
        [SerializeField] TilemapManager tilemapManager;

        // offsets to centralise tilemap around world origin 0,0
        int offsetWidth;
        int offsetHeight;

        bool doDrawTilemap;
        float[,] noiseMap;

        public override void OnNetworkSpawn()
        {
            Debug.Log("WorldManager OnNetworkSpawn");

            worldDetail.OnValueChanged += OnWorldDetailChanged;

            if (!IsServer)
            {
                CreateNoiseMap(worldDetail.Value);
            }
        }

        private void OnWorldDetailChanged(WorldDetail previousValue, WorldDetail newValue)
        {
            Debug.Log("WorldManager OnWorldDetailChanged");

            CreateNoiseMap(newValue);
        }    

        public void UpdateWorld(float scale, int octaves, float persistence, float lacunarity, Vector2Int offset,
            int width, int height, int seed)
        {
            // will trigger OnWorldDetailChanged event
            worldDetail.Value = new WorldDetail(scale, octaves, persistence, lacunarity, offset, width, height, seed);
        }

        // we can't update tilemap from a call initiated by OnValidate so update it separately here
        // this is only a problem on the host but we'll treat the client the same to simplify the code
        private void Update()
        {
            if (doDrawTilemap)
            {
                DrawNoiseAsTiles();
                doDrawTilemap = false;
            }
        }

        private void CreateNoiseMap(WorldDetail worldDetail)
        {
            Debug.Log("WorldManager CreateNoiseMap");

            noiseMap = noiseGenerator.GenerateNoise(worldDetail.Scale, worldDetail.Octaves, worldDetail.Persistence,
                worldDetail.Lacunarity, worldDetail.Offset, worldDetail.WorldWidth, worldDetail.WorldHeight, worldDetail.Seed);

            offsetWidth = worldDetail.WorldWidth - (worldDetail.WorldWidth / 2);
            offsetHeight = worldDetail.WorldHeight - (worldDetail.WorldHeight / 2);

            doDrawTilemap = true;
        }    

        private void DrawNoiseAsTiles()
        {
            Debug.Log("WorldManager DrawNoiseAsTiles");

            tilemapManager.ClearTiles();

            Color color;

            for (int y = 0; y < noiseMap.GetLength(1); y++)
            {
                for (int x = 0; x < noiseMap.GetLength(0); x++)
                {
                    switch (noiseMap[x, y])
                    {
                        case float val when val < 0.4f:
                            color = Color.blue;
                            break;

                        case float val when val < 0.45f:
                            color = Color.yellow;
                            break;

                        case float val when val < 0.6f:
                            color = Color.green;
                            break;

                        case float val when val < 0.9f:
                            color = Color.gray;
                            break;

                        default:
                            color = Color.white;
                            break;
                    }

                    tilemapManager.DrawTile(new Vector3Int(x - offsetWidth, y - offsetHeight, 0), color);
                }
            }
        }

        public override void OnNetworkDespawn()
        {
            worldDetail.OnValueChanged -= OnWorldDetailChanged;
        }
    }
}
