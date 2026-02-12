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
            var sourceCellIsFromSource = _currentSource?.Cells.Contains(_model.SourceCell) == true;
            var targetCellIsFromSource = _currentSource?.Cells.Contains(_model.TargetCell) == true;

            if (sourceCellIsFromSource == targetCellIsFromSource)
            {
                base.Execute(targetIsSource);
                return;
            }

            if (_model.SourceCell.ItemDescription == null || !_model.SourceCell.ItemDescription.IsBuyable)
            {
                return;
            }

            var remaining = _model.SourceCell.ItemDescription.StackSize - _model.TargetCell.Amount;
            var canPut = Math.Min(remaining, _model.SourceCell.Amount);

            if (canPut <= 0)
            {
                return;
            }

            if (_model.TargetCell.ItemDescription != null &&
                _model.TargetCell.ItemDescription.Id != _model.SourceCell.ItemDescription.Id)
            {
                return;
            }

            var price = _model.SourceCell.ItemDescription.Price * canPut;

            var playerBuying = _currentTarget?.Cells.Contains(_model.SourceCell) == true;

            var buyer = playerBuying ? _model.SourceInventory.Value : _model.TargetInventory.Value;
            var seller = playerBuying ? _model.TargetInventory.Value : _model.SourceInventory.Value;

            if (!buyer.CanExtract(_moneyItem, price))
            {
                return;
            }

            buyer.TryTakeItem(_moneyItem, price);
            base.Execute(targetIsSource);
            seller.TryPutItem(_moneyItem, price);
        }
    }
}