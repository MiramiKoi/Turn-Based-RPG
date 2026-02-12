using Runtime.Core;
using Runtime.Units;

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
            var damage = _player.Combat.GetDamage();
            _target.Combat.TakeDamage(damage);
        }
    }
}