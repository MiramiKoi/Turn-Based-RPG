using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Tilemaps;

namespace Runtime.ViewDescriptions.Landscape.Grid
{
    [CreateAssetMenu(fileName = "GridInteractViewDescription", menuName = "ViewDescription/GridInteractViewDescription")]
    public class GridInteractViewDescription : ScriptableObject
    {
        public AssetReferenceT<Tile> TileAsset;
    }
}