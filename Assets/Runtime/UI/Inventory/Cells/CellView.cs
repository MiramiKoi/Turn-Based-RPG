using UnityEngine.UIElements;

namespace Runtime.UI.Inventory.Cells
{
    public class CellView
    {
        public VisualElement Root { get; }
        public VisualElement Icon { get; }
        public Label Amount { get; }

        public CellView(VisualTreeAsset asset)
        {
            Root = asset.CloneTree().Q<VisualElement>("cell");
            Icon = Root.Q<VisualElement>("icon");
            Amount = Root.Q<Label>("amount");
        }
    }
}