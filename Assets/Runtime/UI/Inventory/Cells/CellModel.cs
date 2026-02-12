using System;
using Runtime.Descriptions.Items;

namespace Runtime.UI.Inventory.Cells
{
    public class CellModel
    {
        public event Action OnChanged;
        public event Action OnCellSelected;
        public event Action OnCellDeselected;
        public event Action OnPriceEnabled;
        public event Action OnPriceDisabled;
        public ItemDescription ItemDescription { get; private set; }
        public int Amount { get; private set; }

        public int TryPut(ItemDescription item, int amount)
        {
            if (amount <= 0)
            {
                return 0;
            }

            if (Amount == 0)
            {
                ItemDescription = item;
            }

            if (ItemDescription != item)
            {
                return 0;
            }

            var free = item.StackSize - Amount;
            var put = Math.Min(free, amount);

            Amount += put;

            if (put > 0)
            {
                OnChanged?.Invoke();
            }

            return put;
        }


        public bool TryTake(int amount)
        {
            if (Amount < amount)
            {
                return false;
            }

            Amount -= amount;

            if (Amount == 0)
            {
                Clear();
            }

            OnChanged?.Invoke();

            return true;
        }

        public void CellSelect()
        {
            OnCellSelected?.Invoke();
        }
        
        public void CellDeselect()
        {
            OnCellDeselected?.Invoke();
        }
        
        public void EnablePrice()
        {
            OnPriceEnabled?.Invoke();
        }
        
        public void DisablePrice()
        {
            OnPriceDisabled?.Invoke();
        }
        
        private void Clear()
        {
            ItemDescription = null;
            Amount = 0;
        }
    }
}