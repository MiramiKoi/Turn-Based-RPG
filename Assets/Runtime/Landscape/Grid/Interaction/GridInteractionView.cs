using UnityEngine.Tilemaps;

namespace Runtime.Landscape.Grid.Interaction
{
    public class GridInteractionView
    {
        public Tilemap Tilemap { get; }
        
        public GridInteractionView(Tilemap tilemap)
        {
            Tilemap = tilemap;
        }
    }
}