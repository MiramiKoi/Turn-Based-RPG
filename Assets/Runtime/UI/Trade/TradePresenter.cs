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

        protected override void Execute()
        {
            if (_model.SourceCell == null || _model.TargetCell == null)
            {
                return;
            }

            if (!_model.CurrentItem.IsBuyable)
            {
                return;
            }

            var remaining = _model.CurrentItem.StackSize - _model.TargetCell.Amount;
            var canPut = Math.Min(remaining, _model.CurrentAmount);
            var price = _model.CurrentItem.Price * canPut;

            var takenMoney = _model.TargetInventory.TryTakeItem(_moneyItem, price);

            if (takenMoney < price)
            {
                _model.TargetInventory.TryPutItem(_moneyItem, takenMoney);
                return;
            }

            base.Execute();

            _model.SourceInventory.TryPutItem(_moneyItem, price);
        }
    }
}