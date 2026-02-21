namespace Runtime.UI.Transfer.Handlers
{
    public interface ITransferHandler
    {
        bool CanHandle(TransferModel context);
        void Handle(TransferModel context);
    }
}