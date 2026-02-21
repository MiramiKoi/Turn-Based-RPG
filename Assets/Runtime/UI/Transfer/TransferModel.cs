using Runtime.UI.Inventory;
using Runtime.UI.Inventory.Cells;

namespace Runtime.UI.Transfer
{
    public enum InventoryType
    {
        Player,
        Loot,
        Trader,
        Trash,
        Equipment
    }

    public class TransferModel
    {
        public CellModel SourceCell { get; set; }
        public CellModel TargetCell { get; set; }
        public InventoryModel SourceInventory { get; set; }
        public InventoryModel TargetInventory { get; set; }
        public InventoryType SourceType { get; set; }
        public InventoryType TargetType { get; set; }
    }
}