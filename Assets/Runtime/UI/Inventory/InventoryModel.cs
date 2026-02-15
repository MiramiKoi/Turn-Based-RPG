using System;
using System.Collections.Generic;
using Runtime.Descriptions.Items;
using Runtime.ModelCollections;
using Runtime.UI.Inventory.Cells;

namespace Runtime.UI.Inventory
{
    public class InventoryModel : ISerializable
    {
        public bool Enabled { get; set; }
        public readonly List<CellModel> Cells = new();
        public event Action<CellModel> OnCellChanged;

        public InventoryModel(int size)
        {
            for (var i = 0; i < size; i++)
            {
                var cellModel = new CellModel();
                Cells.Add(cellModel);
            }
        }

        public int TryPutItem(ItemDescription item, int amount)
        {
            if (amount <= 0)
            {
                return 0;
            }

            var remaining = amount;

            foreach (var cell in Cells)
            {
                if (cell.ItemDescription == null || !IsSameItem(cell.ItemDescription, item))
                {
                    continue;
                }

                var put = cell.TryPut(item, remaining);
                remaining -= put;

                if (put > 0)
                {
                    OnCellChanged?.Invoke(cell);
                }

                if (remaining == 0)
                {
                    return amount;
                }
            }

            foreach (var cell in Cells)
            {
                if (cell.ItemDescription != null)
                {
                    continue;
                }

                var put = cell.TryPut(item, remaining);
                remaining -= put;

                if (put > 0)
                {
                    OnCellChanged?.Invoke(cell);
                }

                if (remaining == 0)
                {
                    return amount;
                }
            }

            return amount - remaining;
        }

        public int TryTakeItem(ItemDescription item, int amount)
        {
            if (amount <= 0)
            {
                return 0;
            }

            var remaining = amount;

            foreach (var cell in Cells)
            {
                if (cell.ItemDescription == null || !IsSameItem(cell.ItemDescription, item))
                {
                    continue;
                }

                var take = Math.Min(cell.Amount, remaining);

                if (cell.TryTake(take))
                {
                    remaining -= take;
                    OnCellChanged?.Invoke(cell);
                }

                if (remaining == 0)
                {
                    break;
                }
            }

            return amount - remaining;
        }

        public bool IsEmpty()
        {
            foreach (var cell in Cells)
            {
                if (cell.ItemDescription != null)
                {
                    return false;
                }
            }

            return true;
        }


        public bool CanExtract(ItemDescription item, int amount)
        {
            var remaining = amount;

            foreach (var cell in Cells)
            {
                if (cell.ItemDescription == null || !IsSameItem(cell.ItemDescription, item))
                {
                    continue;
                }

                remaining -= cell.Amount;

                if (remaining <= 0)
                {
                    return true;
                }
            }

            return false;
        }

        private bool IsSameItem(ItemDescription itemA, ItemDescription itemB)
        {
            if (itemA == null || itemB == null)
            {
                return false;
            }

            return itemA.Id == itemB.Id;
        }

        public Dictionary<string, object> Serialize()
        {
            return new Dictionary<string, object>();
        }
    }
}