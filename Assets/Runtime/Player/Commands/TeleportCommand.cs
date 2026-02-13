using Runtime.Core;
using Runtime.Units.Actions;
using UnityEngine;

namespace Runtime.Player.Commands
{
    public class TeleportCommand : IPlayerCommand
    {
        private readonly PlayerModel _player;
        private readonly World _world;
        private readonly Vector2Int _target;

        public TeleportCommand(PlayerModel player, World world, Vector2Int target)
        {
            _player = player;
            _world = world;
            _target = target; //TODO: Должно браться из модели локации(то, куда переместить игрока)
        }
        
        public bool CanExecute()
        {
            return false; //TODO: если игрок в одной точке с координатой входа, которая берётся из модели локации
        }

        public void Execute()
        {
            _world.LootModel.CancelLoot();
            _world.GridModel.ReleaseCell(_player.State.Position.Value);
            //_player.Movement.TeleportTo(_target); //TODO: реализовать
        }
    }
}