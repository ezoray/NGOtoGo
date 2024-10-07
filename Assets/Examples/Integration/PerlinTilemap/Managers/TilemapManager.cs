using UnityEngine;
using UnityEngine.Tilemaps;

namespace NGOtoGo.Examples.Integration.PerlinTilemap
{
    /// <summary>
    /// Handles the drawing (colouring) of the tiles on the tilemap.
    /// </summary>
    public class TilemapManager : MonoBehaviour
    {
        [SerializeField] Tilemap tilemap;
        [SerializeField] SpriteRenderer spriteRenderer;
        Tile tile;

        private void Awake()
        {
            tile = ScriptableObject.CreateInstance<Tile>();
            tile.sprite = spriteRenderer.sprite;
        }

        public void DrawTile(Vector3Int position, Color color)
        {
            tilemap.SetTile(position, tile);
            tilemap.SetTileFlags(position, TileFlags.None);
            tilemap.SetColor(position, color);
        }

        public void ClearTiles()
        {
            tilemap.ClearAllTiles();
        }
    }
}
