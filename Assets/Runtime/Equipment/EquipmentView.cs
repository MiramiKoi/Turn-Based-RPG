using Runtime.UI.Inventory;
using UnityEngine.UIElements;

namespace Runtime.Equipment
{
    public class EquipmentView : InventoryView
    {
        public Label Damage { get; }
        public Label Protection { get; }
        
        public EquipmentView(VisualTreeAsset asset) : base(asset)
        {
            Damage = Root.Q<VisualElement>("damage").Q<Label>("amount");
            Protection = Root.Q<VisualElement>("protection").Q<Label>("amount");
        }
    }
}