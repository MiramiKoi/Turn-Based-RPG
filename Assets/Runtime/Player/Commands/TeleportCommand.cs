using Runtime.Core;
using Runtime.Landscape.Grid.Cell;

namespace Runtime.Player.Commands
{
    public class TeleportCommand : IPlayerCommand
    {
        private readonly PlayerModel _player;
        private readonly World _world;

        public TeleportCommand(PlayerModel player, World world)
        {
            _player = player;
            _world = world;
        }

        public bool CanExecute(CellModel cell)
        {
            return false; //TODO: если игрок в одной точке с координатой входа, которая берётся из модели локации
        }

        public void Execute(CellModel cell)
        {
            _world.LootModel.CancelLoot();
            _world.GridModel.ReleaseCell(_player.State.Position.Value);
            //_player.Movement.TeleportTo(cell.Position); //TODO: реализовать
        }
    }
}