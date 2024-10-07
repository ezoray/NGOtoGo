using UnityEngine;

namespace NGOtoGo.Examples.Integration.PerlinTilemap
{
    /// <summary>
    /// Generates a Perlin noise map.
    /// Code is near identical to that of Sebastian Lague's example (easy to find on Youtube).
    /// You might notice there's a bug in this code, if you find it fix it and let me know :)
    /// </summary>
    public class PerlinNoiseGenerator : MonoBehaviour
    {

        public float[,] GenerateNoise(float scale, int octaves, float persistence, float lacunarity, Vector2Int offset,
            int width, int height, int seed)
        {
            float[,] noiseMap = new float[width, height];

            System.Random prng = new System.Random(seed);
            Vector2[] octaveOffsets = new Vector2[octaves];
            for (int i = 0; i < octaves; i++)
            {
                float offsetX = prng.Next(-100000, 100000) + offset.x;
                float offsetY = prng.Next(-100000, 100000) + offset.y;
                octaveOffsets[i] = new Vector2(offsetX, offsetY);
            }

            if (scale <= 0)
            {
                scale = 0.0001f;
            }

            float maxNoiseHeight = float.MinValue;
            float minNoiseHeight = float.MaxValue;

            float halfWidth = width / 2f;
            float halfHeight = height / 2f;


            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {

                    float amplitude = 1;
                    float frequency = 1;
                    float noiseHeight = 0;

                    for (int i = 0; i < octaves; i++)
                    {
                        float sampleX = (x - halfWidth) / scale * frequency + octaveOffsets[i].x;
                        float sampleY = (y - halfHeight) / scale * frequency + octaveOffsets[i].y;

                        float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;

                        noiseHeight += perlinValue * amplitude;

                        amplitude *= persistence;
                        frequency *= lacunarity;
                    }

                    if (noiseHeight > maxNoiseHeight)
                    {
                        maxNoiseHeight = noiseHeight;
                    }
                    else if (noiseHeight < minNoiseHeight)
                    {
                        minNoiseHeight = noiseHeight;
                    }

                    noiseMap[x, y] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseHeight);
                }
            }

            return noiseMap;
        }
    }
}
