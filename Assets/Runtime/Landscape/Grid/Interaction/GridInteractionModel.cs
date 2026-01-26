using Runtime.Landscape.Grid.Cell;

namespace Runtime.Landscape.Grid.Interaction
{
    public class GridInteractionModel
    {
        public CellModel CurrentCell { get; private set; }

        public void SetCell(CellModel cell)
        {
            CurrentCell = cell;
        }
    }
}