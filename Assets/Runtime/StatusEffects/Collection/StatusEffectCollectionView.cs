using UnityEngine.UIElements;

namespace Runtime.StatusEffects.Collection
{
    public class StatusEffectCollectionView
    {
        public VisualElement Root { get; }
        
        public StatusEffectCollectionView(VisualTreeAsset asset)
        {
            Root = asset.CloneTree().Q<VisualElement>("status-effects-container");
        }
    }
}