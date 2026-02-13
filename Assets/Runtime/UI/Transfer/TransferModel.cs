using Runtime.UI.Inventory;
using Runtime.UI.Inventory.Cells;
using UniRx;

namespace Runtime.UI.Transfer
{
    public enum TransferMode
    {
        Default,
        Trade,
        Trash
    }

    public class TransferModel
    {
        public ReactiveProperty<InventoryModel> SourceInventory { get; } = new();
        public ReactiveProperty<InventoryModel> TargetInventory { get; } = new();
        public ReactiveProperty<TransferMode> Mode { get; } = new(TransferMode.Default);
        public CellModel SourceCell { get; set; }
        public CellModel TargetCell { get; set; }
    }
}