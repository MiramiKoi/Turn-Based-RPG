namespace Runtime.Player
{
    public class PlayerRouteExecutor
    {
        private readonly PlayerModel _player;
        private readonly PlayerRouteStepResolver _resolver;

        public PlayerRouteExecutor(PlayerModel player, PlayerRouteStepResolver resolver)
        {
            _player = player;
            _resolver = resolver;
        }

        public bool ExecuteNext()
        {
            if (!_player.MovementQueueModel.TryDequeue(out var nextCell))
                return false;

            var command = _resolver.Resolve(_player, nextCell);

            if (command == null || !command.CanExecute())
                return false;

            command.Execute();
            return true;
        }
    }
}