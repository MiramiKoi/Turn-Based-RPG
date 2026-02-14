using Runtime.Landscape.Grid.Cell;

namespace Runtime.Player.Commands
{
    public class SkipStepCommand : IPlayerCommand
    {
        public bool CanExecute(CellModel cell)
        {
            return true;
        }

        public void Execute(CellModel cell)
        {
        }
    }
}