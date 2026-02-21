using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Tilemaps;

namespace Runtime.ViewDescriptions.Landscape.Environment
{
    [CreateAssetMenu(fileName = "EnvironmentViewDescription", menuName = "ViewDescription/Landscape/Environment")]
    public class EnvironmentViewDescription : ScriptableObject
    {
        public AssetReferenceT<TileBase> TileAsset;
    }
}