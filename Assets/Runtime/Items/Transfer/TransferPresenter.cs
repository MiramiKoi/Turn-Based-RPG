using Runtime.Common;
using Runtime.Core;

namespace Runtime.Items.Transfer
{
    public class TransferPresenter : IPresenter
    {
        protected readonly TransferModel _model;
        protected readonly World _world;

        public TransferPresenter(TransferModel model, World world)
        {
            _model = model;
            _world = world;
        }
        
        public void Enable()
        {
            _model.OnItemTransfer += HandleTransfer;
        }

        public void Disable()
        {
            _model.OnItemTransfer -= HandleTransfer;
        }

        private void HandleTransfer()
        {
            CellTransfer();
            
            Clear();
        }

        protected virtual void CellTransfer()
        {
            if (_model.SourceCell == null ||  _model.TargetCell == null)
            {
                return;
            }
            
            if (_model.TargetCell.ItemDescription != null && _model.CurrentItem != _model.TargetCell.ItemDescription)
            {
                Clear();
                return;
            }
            
            var remaining = _model.CurrentItem.StackSize - _model.TargetCell.Amount;
            
            if (remaining < _model.CurrentAmount)
            {
                _model.TargetCell.TryPut(_model.CurrentItem, remaining);
                _model.SourceCell.TryTake(remaining);
            }
            else
            {
                _model.TargetCell.TryPut(_model.CurrentItem, _model.CurrentAmount);
                _model.SourceCell.TryTake(_model.CurrentAmount);
            }
        }
        
        protected void Clear()
        {
            _model.SourceCell = null;
            _model.TargetCell = null;
            _model.ClearItem();
        }
    }
}