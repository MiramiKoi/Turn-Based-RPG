using Runtime.Core;
using Runtime.Descriptions.Items;
using Runtime.Items.Transfer;

namespace Runtime.Items.Trade
{
    public class TradePresenter : TransferPresenter
    {
        private readonly ItemDescription _moneyItem;
        
        public TradePresenter(TransferModel model, World world) : base(model, world)
        {
            _world.WorldDescription.ItemCollection.Descriptions.TryGetValue("money", out var moneyItem);
            _moneyItem = moneyItem;
        }

        protected override void CellTransfer()
        {
            if (_model.SourceCell == null || _model.TargetCell == null)
            {
                return;
            }

            if (!_model.CurrentItem.IsBuyable)
            {
                Clear();
                return;
            }
    
            var price = _model.CurrentItem.Price * _model.CurrentAmount;
            
            var takenMoney = _model.TargetInventory.TryTakeItem(_moneyItem, price);
    
            if (takenMoney < price)
            {
                _model.TargetInventory.TryPutItem(_moneyItem, takenMoney);
                return;
            }
            
            base.CellTransfer();
            
            _model.SourceInventory.TryPutItem(_moneyItem, price);
        }
    }
}