using UnityEngine;
using UnityEngine.UIElements;

namespace Runtime.ViewDescriptions.Inventory
{
    [CreateAssetMenu(fileName = "InventoryViewDescription", menuName = "ViewDescription/Inventory/InventoryViewDescription")]
    public class InventoryViewDescription : ScriptableObject
    {
        public VisualTreeAsset InventoryAsset;
        public VisualTreeAsset CellAsset;
    }
}