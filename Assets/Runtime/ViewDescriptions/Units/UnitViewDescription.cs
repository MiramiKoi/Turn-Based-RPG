using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Runtime.ViewDescriptions.Units
{
    [CreateAssetMenu(fileName = "UnitViewDescription", menuName = "ViewDescription/Units/Controllable")]
    public class UnitViewDescription : ScriptableObject
    {
        public string Id => name;
        public AssetReferenceGameObject Prefab;
    }
}