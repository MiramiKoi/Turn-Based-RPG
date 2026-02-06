using UnityEngine.UIElements;

namespace Runtime.UI.Inventory
{
    public class InventoryView
    {
        public VisualElement Root { get; }
        public VisualElement CellsContainer { get; }
        
        public InventoryView(VisualTreeAsset asset)
        {
            Root = asset.CloneTree().Q<VisualElement>("inventory-panel");
            CellsContainer = Root.Q<VisualElement>("cells-container");
        }
    }
}