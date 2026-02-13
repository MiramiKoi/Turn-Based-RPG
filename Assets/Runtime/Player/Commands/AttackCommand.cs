using Runtime.Core;
using Runtime.Units;
using Runtime.Units.Rotation;

namespace Runtime.Player.Commands
{
    public class AttackCommand : IPlayerCommand
    {
        private readonly World _world;
        private readonly PlayerModel _player;
        private readonly UnitModel _target;

        public AttackCommand(PlayerModel player, UnitModel target, World world)
        {
            _player = player;
            _target = target;
            _world = world;
        }

        public bool CanExecute()
        {
            return !_target.IsDead &&
                   _player.Combat.CanAttack(_target.State.Position.Value);
        }

        public void Execute()
        {
            _world.LootModel.CancelLoot();
            
            if (_player.State.Position.Value.x != _target.State.Position.Value.x)
                _player.Movement.Rotate(_target.State.Position.Value.x < _player.State.Position.Value.x ? UnitDirection.Left : UnitDirection.Right);
            
            var damage = _player.Combat.GetDamage();
            _target.Combat.TakeDamage(damage);
        }
    }
}