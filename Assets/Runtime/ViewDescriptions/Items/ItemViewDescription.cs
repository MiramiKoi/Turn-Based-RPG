using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Runtime.ViewDescriptions.Items
{
    [CreateAssetMenu(fileName = "ItemViewDescription", menuName = "ViewDescription/Items/ItemViewDescription")]
    public class ItemViewDescription : ScriptableObject
    {
        public string Id => name;
        public AssetReferenceT<Sprite> Icon;
        public string Title;
        public string Description;
    }
}