using Runtime.Core;
using Runtime.Landscape.Grid.Cell;
using Runtime.Units;

namespace Runtime.Player.Commands
{
    public class LootCommand : IPlayerCommand
    {
        private readonly PlayerModel _player;
        private readonly World _world;

        public LootCommand(PlayerModel player, World world)
        {
            _player = player;
            _world = world;
        }

        public bool CanExecute(CellModel cell)
        {
            if (cell.Unit is not UnitModel target)
                return false;

            return _player.Mode != PlayerMode.Attack && (target.IsDead || target.Description.Fraction == "trader");
        }

        public void Execute(CellModel cell)
        {
            var target = (UnitModel)cell.Unit;
            _world.LootModel.RequestLoot(target);
        }
    }
}