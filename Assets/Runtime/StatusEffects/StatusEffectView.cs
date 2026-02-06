using UnityEngine.UIElements;

namespace Runtime.StatusEffects
{
    public class StatusEffectView
    {
        public VisualElement Root { get; }
        public Label Counter { get; }
        public Image Icon { get; }
        
        public StatusEffectView(VisualTreeAsset asset)
        {
            Root = asset.CloneTree().Q<VisualElement>("status-effect-root");
            Icon = Root.Q<Image>("icon");
            Counter = Root.Q<Label>("counter");
        }
    }
}