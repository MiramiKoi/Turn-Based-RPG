using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Tilemaps;

namespace Runtime.ViewDescriptions.Landscape.Grid
{
    [CreateAssetMenu(fileName = "GridIndicationViewDescription", menuName = "ViewDescription/GridIndicationViewDescription")]
    public class GridIndicationViewDescription : ScriptableObject
    {
        public AssetReferenceT<Tile> CellCursorAsset;
        public AssetReferenceT<Tile> RoutePointAsset;
    }
}