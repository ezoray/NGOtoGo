using Unity.Netcode;
using UnityEngine;

namespace NGOtoGo.Examples
{
    /// <summary>
    /// Container for the values required to generate the noise map.
    /// This is passed as a network variable from host to client.
    /// </summary>
    public struct WorldDetail : INetworkSerializeByMemcpy
    {
        public float Scale;
        public int Octaves;
        public float Persistence;
        public float Lacunarity;
        public Vector2Int Offset;
        public int WorldWidth;
        public int WorldHeight;
        public int Seed;

        public WorldDetail(float scale, int octaves, float persistence, float lacunarity, Vector2Int offset, int worldWidth,
            int worldHeight, int seed)
        {
            Scale = scale;
            Octaves = octaves;
            Persistence = persistence;
            Lacunarity = lacunarity;
            Offset = offset;
            WorldWidth = worldWidth;
            WorldHeight = worldHeight;
            Seed = seed;
        }
    }
}
