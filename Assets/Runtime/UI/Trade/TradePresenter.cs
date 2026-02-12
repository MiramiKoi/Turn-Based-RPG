using System;
using Runtime.Core;
using Runtime.Descriptions.Items;
using Runtime.UI.Transfer;

namespace Runtime.UI.Trade
{
    public class TradePresenter : TransferPresenter
    {
        private readonly ItemDescription _moneyItem;

        public TradePresenter(TransferModel model, World world) : base(model)
        {
            world.WorldDescription.ItemCollection.Descriptions.TryGetValue("money", out _moneyItem);
        }

        protected override void Execute(bool targetIsSource)
        {
            if (_model.SourceCell == _model.TargetCell || (targetIsSource && _currentSource == _currentTarget))
            {
                base.Execute(targetIsSource);
                return;
            }

            if (_model.SourceCell.ItemDescription is not { IsBuyable: true })
            {
                return;
            }

            var remaining = _model.SourceCell.ItemDescription.StackSize - _model.TargetCell.Amount;
            var canPut = Math.Min(remaining, _model.SourceCell.Amount);
            var price = _model.SourceCell.ItemDescription.Price * canPut;
            
            var playerBuying = _currentTarget?.Cells.Contains(_model.SourceCell) == true;
    
            var buyer = playerBuying ? _model.SourceInventory.Value : _model.TargetInventory.Value;
            var seller = playerBuying ? _model.TargetInventory.Value : _model.SourceInventory.Value;

            var takenMoney = buyer.TryTakeItem(_moneyItem, price);

            if (takenMoney < price)
            {
                buyer.TryPutItem(_moneyItem, takenMoney);
                return;
            }

            base.Execute(targetIsSource);
            seller.TryPutItem(_moneyItem, price);
        }
    }
}