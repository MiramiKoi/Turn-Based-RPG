using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Tilemaps;

namespace Runtime.ViewDescriptions.Landscape.Surface
{
    [CreateAssetMenu(fileName = "SurfaceViewDescription", menuName = "ViewDescription/Landscape/Surface")]
    public class SurfaceViewDescription : ScriptableObject
    {
        public AssetReferenceT<Tile> TileAsset;
    }
}