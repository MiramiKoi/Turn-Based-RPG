using System;
using Runtime.Descriptions.Items;

namespace Runtime.UI.Transfer.Handlers
{
    public class TradeHandler : BaseTransferHandler
    {
        private readonly ItemDescription _moneyItem;

        public TradeHandler(ItemDescription moneyItem)
        {
            _moneyItem = moneyItem;
        }

        public override bool CanHandle(TransferModel context)
        {
            if (context.SourceType == InventoryType.Equipment || context.TargetType == InventoryType.Equipment)
            {
                return false;
            }
            
            return context.SourceType == InventoryType.Trader || context.TargetType == InventoryType.Trader;
        }

        public override void Handle(TransferModel context)
        {
            if (context.SourceCell.ItemDescription is not { IsBuyable: true })
            {
                return;
            }

            var remaining = context.SourceCell.ItemDescription.StackSize - context.TargetCell.Amount;
            var canPut = Math.Min(remaining, context.SourceCell.Amount);

            if (canPut <= 0)
            {
                return;
            }

            if (context.TargetCell.ItemDescription != null &&
                context.TargetCell.ItemDescription.Id != context.SourceCell.ItemDescription.Id)
            {
                return;
            }

            var price = context.SourceCell.ItemDescription.Price * canPut;

            var playerBuying = context.TargetInventory?.Cells.Contains(context.SourceCell) == true;

            var buyer = playerBuying ? context.SourceInventory : context.TargetInventory;
            var seller = playerBuying ? context.TargetInventory : context.SourceInventory;

            if (!buyer.CanExtract(_moneyItem, price))
            {
                return;
            }

            buyer.TryTakeItem(_moneyItem, price);
            
            base.Handle(context);
        
            seller.TryPutItem(_moneyItem, price);
        }
    }
}