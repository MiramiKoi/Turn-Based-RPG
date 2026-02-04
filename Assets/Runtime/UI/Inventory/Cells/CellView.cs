using UnityEngine.UIElements;

namespace Runtime.UI.Inventory.Cells
{
    public class CellView
    {
        public VisualElement Root { get; }
        public VisualElement Icon { get; }

        public CellView(VisualTreeAsset asset)
        {
            Root = asset.CloneTree().Q<VisualElement>("cell");
            Icon = Root.Q<VisualElement>("icon");
        }
    }
}