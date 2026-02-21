using Runtime.Landscape.Grid.Cell;

namespace Runtime.Player.Commands
{
    public interface IPlayerCommand
    {
        bool CanExecute(CellModel cell);
        void Execute(CellModel cell);
    }
}