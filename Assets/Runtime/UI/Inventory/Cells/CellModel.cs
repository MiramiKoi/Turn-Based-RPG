using System;

namespace Runtime.UI.Inventory.Cells
{
    public class CellModel
    {
        public event Action OnChanged;
        public IItemDescription ItemDescription { get; private set; }
        public int Amount { get; private set; }

        public bool TryPut(IItemDescription itemDescription, int amount)
        {
            if (Amount == 0)
            {
                if (amount > itemDescription.StackSize)
                {
                    return false;
                }

                ItemDescription = itemDescription;
            }

            if (Amount + amount > itemDescription.StackSize)
            {
                return false;
            }

            Amount += amount;
            OnChanged?.Invoke();

            return true;
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