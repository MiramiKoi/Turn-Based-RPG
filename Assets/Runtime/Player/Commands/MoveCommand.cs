using Runtime.Core;
using Runtime.Units.Actions;
using UnityEngine;

namespace Runtime.Player.Commands
{
    public class MoveCommand : IPlayerCommand
    {
        private readonly PlayerModel _player;
        private readonly World _world;
        private readonly Vector2Int _target;

        public MoveCommand(PlayerModel player, World world, Vector2Int target)
        {
            _player = player;
            _world = world;
            _target = target;
        }

        public bool CanExecute()
        {
            return _player.ActionBlocker.CanExecute(UnitActionType.Move)
                   && _world.GridModel.TryPlace(_player, _target);
        }

        public void Execute()
        {
            _world.LootModel.CancelLoot();
            _world.GridModel.ReleaseCell(_player.State.Position.Value);
            _player.Movement.MoveTo(_target);
        }
    }
}