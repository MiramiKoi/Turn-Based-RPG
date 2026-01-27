using UnityEngine.Tilemaps;

namespace Runtime.Landscape.Grid.Indication
{
    public class GridIndicationView
    {
        public Tilemap Tilemap { get;}
        
        public GridIndicationView(Tilemap tilemap)
        {
            Tilemap = tilemap;
        }
    }
}