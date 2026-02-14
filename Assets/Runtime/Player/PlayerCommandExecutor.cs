using System.Collections.Generic;
using Runtime.Core;
using Runtime.Player.Commands;

namespace Runtime.Player
{
    public class PlayerCommandExecutor
    {
        private readonly PlayerModel _player;
        private readonly World _world;
        private readonly List<IPlayerCommand> _commands;

        public PlayerCommandExecutor(PlayerModel player, World world)
        {
            _player = player;
            _world = world;
            
            _commands = new List<IPlayerCommand>
            {
                new TeleportCommand(_player, _world),
                new MoveCommand(_player, _world),
                new LootCommand(_world),
                new AttackCommand(_player, _world),
                new SkipStepCommand()
            };
        }

        public bool ExecuteNext()
        {
            if (!_player.MovementQueueModel.TryDequeue(out var nextCell))
                return false;

            var cell = _world.GridModel.GetCell(nextCell);

            foreach (var command in _commands)
            {
                if (command.CanExecute(cell))
                {
                    command.Execute(cell);
                    return true;
                }
            }

            return false;
        }
    }
}