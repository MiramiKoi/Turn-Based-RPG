using System;
using Runtime.Descriptions.Items;
using Runtime.UI.Inventory.Cells;

namespace Runtime.UI.Swap
{
    public class SwapModel
    {
        public event Action<CellModel> OnSwap;
        public CellModel SourceCell { get; private set; }
        public ItemDescription CurrentItem { get; private set; }
        public int CurrentAmount { get; private set; }

        public bool TrySetCell(CellModel sourceCell)
        {
            if (sourceCell.ItemDescription == null)
            {
                return false;
            }

            if (SourceCell == null)
            {
                SourceCell = sourceCell;
                CurrentItem = sourceCell.ItemDescription;
                CurrentAmount = sourceCell.Amount;
                return true;
            }

            return false;
        }

        public void Swap(CellModel targetCell)
        {
            if (CurrentItem == null || SourceCell == null)
            {
                return;
            }

            OnSwap?.Invoke(targetCell);
        }

        public void Clear()
        {
            SourceCell = null;
            CurrentItem = null;
            CurrentAmount = 0;
        }
    }
}