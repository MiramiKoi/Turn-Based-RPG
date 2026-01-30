using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Runtime.ViewDescriptions.Unit
{
    [CreateAssetMenu(fileName = "UnitViewDescription", menuName = "ViewDescription/Units/Unit")]
    public class UnitViewDescription : ScriptableObject
    {
        public string Id => name;
        public AssetReferenceT<GameObject> Prefab;
    }
}