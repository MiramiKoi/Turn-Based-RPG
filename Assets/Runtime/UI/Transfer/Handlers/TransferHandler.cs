namespace Runtime.UI.Transfer.Handlers
{
    public class TransferHandler : BaseTransferHandler
    {
        public override bool CanHandle(TransferModel context)
        {
            if (context.SourceType == InventoryType.Equipment || context.TargetType == InventoryType.Equipment)
            {
                return false;
            }

            return context.SourceInventory != context.TargetInventory;
        }
    }
}