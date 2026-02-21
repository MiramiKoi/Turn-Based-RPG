using Runtime.Core;
using Runtime.Landscape.Grid.Cell;
using Runtime.Units;
using Runtime.Units.Actions;
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
            if (_player.Mode == PlayerMode.Attack)
                return true;
            
            if (cell.Unit is not UnitModel target)
                return false;
            
            if (!_player.ActionBlocker.CanExecute(UnitActionType.Attack))
                return false;

            return target != _player 
                   && !target.IsDead 
                   && target.Description.EnemyFractions.Contains(_player.Description.Fraction)
                   && _player.Combat.CanAttack(target.State.Position.Value);
        }

        public void Execute(CellModel cell)
        {
            var target = cell.Unit as UnitModel;
            _world.LootModel.CancelLoot();

            if (_player.State.Position.Value.x != cell.Position.x)
                _player.Movement.Rotate(cell.Position.x < _player.State.Position.Value.x
                    ? UnitDirection.Left
                    : UnitDirection.Right);

            var damage = _player.Combat.GetDamage();
            if (target != null && _player.Combat.CanAttack(target.State.Position.Value))
            {
                target.Combat.TakeDamage(damage);
            }
        }
    }
}