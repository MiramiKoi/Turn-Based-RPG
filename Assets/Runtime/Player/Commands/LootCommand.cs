using Runtime.Core;
using Runtime.Units;

namespace Runtime.Player.Commands
{
    public class LootCommand : IPlayerCommand
    {
        private readonly World _world;
        private readonly UnitModel _target;

        public LootCommand(World world, UnitModel target)
        {
            _world = world;
            _target = target;
        }

        public bool CanExecute()
        {
            return _target.IsDead || _target.Description.Fraction == "trader";
        }

        public void Execute()
        {
            _world.LootModel.RequestLoot(_target);
        }
    }
}