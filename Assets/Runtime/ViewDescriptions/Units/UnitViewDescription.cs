using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Runtime.ViewDescriptions.Units
{
    [CreateAssetMenu(fileName = "UnitViewDescription", menuName = "ViewDescription/Units/Unit")]
    public class UnitViewDescription : ScriptableObject
    {
        public string Id => name;
        public AssetReferenceT<GameObject> Prefab;
    }
}