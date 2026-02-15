namespace Runtime.UI.Transfer.Handlers
{
    public class SwapHandler : BaseTransferHandler
    {
        public override bool CanHandle(TransferModel context)
        {
            return context.SourceInventory == context.TargetInventory;
        }
    }
}