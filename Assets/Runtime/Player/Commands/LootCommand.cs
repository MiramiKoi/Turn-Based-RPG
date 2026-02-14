using Runtime.Core;
using Runtime.Landscape.Grid.Cell;
using Runtime.Units;

namespace Runtime.Player.Commands
{
    public class LootCommand : IPlayerCommand
    {
        private readonly World _world;

        public LootCommand(World world)
        {
            _world = world;
        }

        public bool CanExecute(CellModel cell)
        {
            if (cell.Unit is not UnitModel target)
                return false;
                
            return target.IsDead || target.Description.Fraction == "trader";
        }

        public void Execute(CellModel cell)
        {
            var target = (UnitModel)cell.Unit;
            _world.LootModel.RequestLoot(target);
        }
    }
}