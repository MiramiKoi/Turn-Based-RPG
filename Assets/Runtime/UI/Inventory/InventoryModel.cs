using System.Collections.Generic;
using Runtime.UI.Inventory.Cells;

namespace Runtime.UI.Inventory
{
    public class InventoryModel
    {
        public readonly List<CellModel> Cells = new();
        public bool Enabled;

        public InventoryModel(int size)
        {
            for (var i = 0; i < size; i++)
            {
                var cellModel = new CellModel();
                Cells.Add(cellModel);
            }
        }
    }
}