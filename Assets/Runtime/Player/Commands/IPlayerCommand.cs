namespace Runtime.Player.Commands
{
    public interface IPlayerCommand
    {
        bool CanExecute();
        void Execute();
    }
}