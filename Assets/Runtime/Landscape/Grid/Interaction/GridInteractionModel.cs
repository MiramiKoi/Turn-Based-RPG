using System;
using Runtime.Landscape.Grid.Cell;

namespace Runtime.Landscape.Grid.Interaction
{
    public class GridInteractionModel
    {
        public CellModel CurrentCell { get; private set; }
        public event Action<CellModel> OnCellChanged;
        
        public void SetCell(CellModel cell)
        {
            CurrentCell = cell;
            
            OnCellChanged?.Invoke(cell);
        }
    }
}