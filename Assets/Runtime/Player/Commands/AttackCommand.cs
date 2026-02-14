using Runtime.Core;
using Runtime.Landscape.Grid.Cell;
using Runtime.Units;
using Runtime.Units.Rotation;

namespace Runtime.Player.Commands
{
    public class AttackCommand : IPlayerCommand
    {
        private readonly World _world;
        private readonly PlayerModel _player;

        public AttackCommand(PlayerModel player, World world)
        {
            _player = player;
            _world = world;
        }

        public bool CanExecute(CellModel cell)
        {
            if (cell.Unit is not UnitModel target)
                return false;
            
            return target != _player &&
                   !target.IsDead &&
                   _player.Combat.CanAttack(target.State.Position.Value);
        }

        public void Execute(CellModel cell)
        {
            var target = (UnitModel)cell.Unit;
            _world.LootModel.CancelLoot();
            
            if (_player.State.Position.Value.x != target.State.Position.Value.x)
                _player.Movement.Rotate(target.State.Position.Value.x < _player.State.Position.Value.x ? UnitDirection.Left : UnitDirection.Right);
            
            var damage = _player.Combat.GetDamage();
            target.Combat.TakeDamage(damage);
        }
    }
}