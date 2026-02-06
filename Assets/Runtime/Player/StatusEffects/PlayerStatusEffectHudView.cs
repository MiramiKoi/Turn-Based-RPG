using UnityEngine.UIElements;

namespace Runtime.Player.StatusEffects
{
    public class PlayerStatusEffectHudView
    {
        public VisualElement Root { get; }
        
        public PlayerStatusEffectHudView(VisualTreeAsset asset)
        {
            Root = asset.CloneTree().Q<VisualElement>("status-effects-container");
        }
    }
}