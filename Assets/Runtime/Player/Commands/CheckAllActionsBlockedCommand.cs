using Runtime.Landscape.Grid.Cell;
using Runtime.Units.Actions;

namespace Runtime.Player.Commands
{
    public sealed class CheckAllActionsBlockedCommand : IPlayerCommand
    {
        private readonly PlayerModel _player;

        public CheckAllActionsBlockedCommand(PlayerModel player)
        {
            _player = player;
        }

        public bool CanExecute(CellModel cell)
        {
            return !_player.ActionBlocker.CanExecute(UnitActionType.All);
        }

        public void Execute(CellModel cell)
        {
            _player.Mode = PlayerMode.Battle;
        }
    }
}