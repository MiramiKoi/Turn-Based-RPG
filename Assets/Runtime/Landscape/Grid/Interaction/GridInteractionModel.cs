using System;
using Runtime.Landscape.Grid.Cell;
using UniRx;

namespace Runtime.Landscape.Grid.Interaction
{
    public class GridInteractionModel
    {
        public event Action OnCurrentCellChanged;

        public CellModel CurrentCell { get; private set; }
        public BoolReactiveProperty IsActive { get; set; } = new(true);

        public void SetCell(CellModel cell)
        {
            CurrentCell = cell;
            OnCurrentCellChanged?.Invoke();
        }
    }
}