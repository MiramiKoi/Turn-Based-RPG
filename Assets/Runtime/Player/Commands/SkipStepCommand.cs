using Runtime.Landscape.Grid.Cell;

namespace Runtime.Player.Commands
{
    public class SkipStepCommand : IPlayerCommand
    {
        private readonly PlayerModel _player;

        public SkipStepCommand(PlayerModel player)
        {
            _player = player;
        }

        public bool CanExecute(CellModel cell)
        {
            return true;
        }

        public void Execute(CellModel cell)
        {
            _player.Mode = PlayerMode.Battle;
        }
    }
}