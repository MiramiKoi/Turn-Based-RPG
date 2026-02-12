using Runtime.Common;

namespace Runtime.Items.Transfer
{
    public class TransferPresenter : IPresenter
    {
        private readonly TransferModel _model;

        public TransferPresenter(TransferModel model)
        {
            _model = model;
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

        private void CellTransfer()
        {
            if (_model.SourceCell == null || _model.TargetCell == null)
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

        private void Clear()
        {
            _model.SourceCell = null;
            _model.TargetCell = null;
            _model.ClearItem();
        }
    }
}