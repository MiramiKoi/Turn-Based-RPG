using Runtime.Units;

namespace Runtime.Player.Commands
{
    public class AttackCommand : IPlayerCommand
    {
        private readonly PlayerModel _player;
        private readonly UnitModel _target;

        public AttackCommand(PlayerModel player, UnitModel target)
        {
            _player = player;
            _target = target;
        }

        public bool CanExecute()
            => !_target.IsDead &&
               _player.Combat.CanAttack(_target.State.Position.Value);

        public void Execute()
        {
            var damage = _player.Combat.GetDamage();
            _target.Combat.TakeDamage(damage);
        }
    }
}