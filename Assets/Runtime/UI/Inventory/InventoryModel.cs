using System;
using System.Collections.Generic;
using System.Linq;
using Runtime.UI.Inventory.Cells;

namespace Runtime.UI.Inventory
{
    public class InventoryModel
    {
        public event Action OnRemoveItem;
        public bool Enabled;
        public readonly List<CellModel> Cells = new();

        public InventoryModel(int size)
        {
            for (var i = 0; i < size; i++)
            {
                var cellModel = new CellModel();
                Cells.Add(cellModel);
            }
        }

        public bool TryPutItem(IItemDescription item, int amount)
        {
            if (!CanFit(item, amount, out var targets))
            {
                return false;
            }
            
            var remaining = amount;

            foreach (var (cell, free) in targets)
            {
                var toAdd = Math.Min(free, remaining);
                cell.TryPut(item, toAdd);

                remaining -= toAdd;

                if (remaining == 0)
                {
                    return true;
                }
            }

            return false;
        }
        
        public bool TryTakeItem(IItemDescription item, int amount)
        {
            if (!CanExtract(item, amount, out var targets))
            {
                return false;
            }
            
            var remaining = amount;

            foreach (var (cell, free) in targets)
            {
                var toRemove = Math.Min(free, remaining);
                cell.TryTake(toRemove);
                remaining -= toRemove;
            }
            
            OnRemoveItem?.Invoke();
            return true;
        }
        
        public bool CanExtract(IItemDescription item, int amount, out List<(CellModel cell, int amount)> targets)
        {
            targets = new List<(CellModel, int)>();
            var remaining = amount;

            for (var i = Cells.Count - 1; i >= 0; i--)
            {
                var cell = Cells.ElementAt(i);

                if (cell.ItemDescription != null && IsSameItem(cell.ItemDescription, item))
                {
                    var toRemove = Math.Min(cell.Amount, remaining);

                    if (toRemove > 0)
                    {
                        targets.Add((cell, toRemove));
                        remaining -= toRemove;

                        if (remaining == 0)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        public bool CanFit(IItemDescription item, int amount, out List<(CellModel cell, int free)> targets)
        {
            targets = new List<(CellModel, int)>();
            var remaining = amount;

            foreach (var cell in Cells)
            {
                if (cell.ItemDescription != null && IsSameItem(cell.ItemDescription, item))
                {
                    var free = cell.ItemDescription.StackSize - cell.Amount;

                    if (free > 0)
                    {
                        targets.Add((cell, free));
                        remaining -= Math.Min(free, remaining);

                        if (remaining <= 0)
                        {
                            return true;
                        }
                    }
                }
            }
            foreach (var cell in Cells)
            {
                if (cell.ItemDescription == null)
                {
                    var free = item.StackSize;
                    targets.Add((cell, free));

                    remaining -= Math.Min(free, remaining);

                    if (remaining <= 0)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private bool IsSameItem(IItemDescription itemA, IItemDescription itemB)
        {
            if (itemA == null || itemB == null)
            {
                return false;
            }

            return itemA.Id == itemB.Id;
        }
    }
}