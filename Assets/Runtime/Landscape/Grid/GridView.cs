using UnityEngine.Tilemaps;

namespace Runtime.Landscape.Grid
{
    public class GridView
    {
        public Tilemap Tilemap { get; }
        
        public GridView(Tilemap tilemap)
        {
            Tilemap = tilemap;
        }
    }
}