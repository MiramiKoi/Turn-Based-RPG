using System;
using Runtime.Landscape.Grid.Cell;

namespace Runtime.Landscape.Grid.Interaction
{
    public class GridInteractionModel
    {
        public event Action OnCurrentCellChanged;
        
        public CellModel CurrentCell { get; private set; }
        public bool IsActive { get; set; } = true;

        public void SetCell(CellModel cell)
        {
            CurrentCell = cell;
            OnCurrentCellChanged?.Invoke();
        }
    }
}