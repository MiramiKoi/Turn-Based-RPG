namespace Runtime.UI.Transfer.Handlers
{
    public abstract class BaseTransferHandler : ITransferHandler
    {
        public abstract bool CanHandle(TransferModel context);

        public virtual void Handle(TransferModel context)
        {
            if (context.SourceCell == context.TargetCell)
            {
                return;
            }

            var sourceItem = context.SourceCell.ItemDescription;
            var sourceAmount = context.SourceCell.Amount;
            var targetItem = context.TargetCell.ItemDescription;
            var targetAmount = context.TargetCell.Amount;

            if (targetItem != null && targetItem.Id == sourceItem.Id)
            {
                var put = context.TargetCell.TryPut(sourceItem, sourceAmount);
                context.SourceCell.TryTake(put);
                return;
            }

            context.SourceCell.TryTake(sourceAmount);
            context.TargetCell.TryTake(targetAmount);
            context.SourceCell.TryPut(targetItem, targetAmount);
            context.TargetCell.TryPut(sourceItem, sourceAmount);
        }
    }
}