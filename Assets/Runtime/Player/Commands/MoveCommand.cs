using Runtime.Core;
using Runtime.Landscape.Grid.Cell;
using Runtime.Units.Actions;

namespace Runtime.Player.Commands
{
    public class MoveCommand : IPlayerCommand
    {
        private readonly PlayerModel _player;
        private readonly World _world;

        public MoveCommand(PlayerModel player, World world)
        {
            _player = player;
            _world = world;
        }

        public bool CanExecute(CellModel cell)
        {
            return cell.Unit == null
                   && _player.ActionBlocker.CanExecute(UnitActionType.Move)
                   && _world.GridModel.TryPlace(_player, cell.Position);
        }

        public void Execute(CellModel cell)
        {
            _world.LootModel.CancelLoot();
            _world.GridModel.ReleaseCell(_player.State.Position.Value);
            _player.Movement.MoveTo(cell.Position);
        }
    }
}