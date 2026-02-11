using System;
using Runtime.Descriptions.Items;

namespace Runtime.UI.Inventory.Cells
{
    public class CellModel
    {
        public event Action OnChanged;
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

        private void Clear()
        {
            ItemDescription = null;
            Amount = 0;
        }
    }
}