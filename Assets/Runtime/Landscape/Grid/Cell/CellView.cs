using UnityEngine.Tilemaps;

namespace Runtime.Landscape.Grid.Cell
{
    public class CellView
    {
        public readonly Tilemap Tilemap;
        
        public CellView(Tilemap tilemap)
        {
            Tilemap = tilemap;
        }
    }
}