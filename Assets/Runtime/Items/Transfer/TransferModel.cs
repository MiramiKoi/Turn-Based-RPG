using System;
using Runtime.Descriptions.Items;
using Runtime.UI.Inventory;
using Runtime.UI.Inventory.Cells;

namespace Runtime.Items.Transfer
{
    public class TransferModel
    {
        public event Action OnItemTransfer;
        public CellModel SourceCell { get; set; }
        public CellModel TargetCell { get; set; }
        public InventoryModel SourceInventory { get; set; }
        public InventoryModel TargetInventory { get; set; }
        public ItemDescription CurrentItem { get; private set; }
        public int CurrentAmount { get; private set; }

        public bool TrySetCell(CellModel sourceCell, InventoryModel sourceInventory)
        {
            if (sourceCell.ItemDescription == null)
            {
                return false;
            }

            if (SourceCell == null)
            {
                SourceCell = sourceCell;
                SourceInventory = sourceInventory;
                CurrentItem = SourceCell.ItemDescription;
                CurrentAmount = SourceCell.Amount;
                return true;
            }

            return false;
        }
        
        public void Transfer()
        {
            if (CurrentItem == null || CurrentAmount == 0)
            {
                return;
            }

            OnItemTransfer?.Invoke();
        }

        public void ClearItem()
        {
            CurrentItem = null;
            CurrentAmount = 0;
            SourceInventory = null;
            TargetInventory = null;
        }
    }
}