using System.Collections.Generic;
using Runtime.UI.Inventory;
using UnityEngine.UIElements;

namespace Runtime.Equipment
{
    public class EquipmentView : InventoryView
    {
        public Dictionary<string, Label> Stats { get; } = new();

        public EquipmentView(VisualTreeAsset asset) : base(asset)
        {
            Stats["attack_damage"] = Root.Q<VisualElement>("damage").Q<Label>("amount");
            Stats["protection"] = Root.Q<VisualElement>("protection").Q<Label>("amount");
        }
    }
}