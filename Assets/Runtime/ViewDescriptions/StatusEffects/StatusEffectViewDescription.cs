using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UIElements;

namespace Runtime.ViewDescriptions.StatusEffects
{
    [CreateAssetMenu(fileName = "StatusEffectViewDescription", menuName = "ViewDescription/StatusEffects/StatusEffect")]
    public class StatusEffectViewDescription : ScriptableObject
    {
        public string Id => name;
        public AssetReferenceSprite Icons;
        public AssetReferenceT<VisualTreeAsset> StatusEffectViewAsset;
    }
}