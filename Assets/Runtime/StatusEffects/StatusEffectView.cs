using UnityEngine.UIElements;

namespace Runtime.StatusEffects
{
    public class StatusEffectView
    {
        public VisualElement Root { get; }
        public Label TurnsCounter { get; }
        public Label StackCounter { get; }
        public Image Icon { get; }
        
        public StatusEffectView(VisualTreeAsset asset)
        {
            Root = asset.CloneTree().Q<VisualElement>("status-effect-root");
            Icon = Root.Q<Image>("icon");
            TurnsCounter = Root.Q<Label>("turns_counter");
            StackCounter = Root.Q<Label>("stack_counter");
        }
    }
}