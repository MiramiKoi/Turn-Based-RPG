using System;
using Runtime.Common;

namespace Runtime.UI.Transfer
{
    public class TransferPresenter : IPresenter
    {
        protected readonly TransferModel _model;

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
            _model.ClearItem();
        }

        private void HandleTransfer()
        {
            Execute();
            _model.ClearItem();
        }

        protected virtual void Execute()
        {
            if (_model.SourceCell == null || _model.TargetCell == null)
            {
                return;
            }

            if (_model.SourceInventory == _model.TargetInventory)
            {
                Swap();
                return;
            }

            Move();
        }

        private void Swap()
        {
            if (_model.SourceCell == _model.TargetCell)
            {
                return;
            }

            var sourceItem = _model.SourceCell.ItemDescription;
            var sourceAmount = _model.SourceCell.Amount;
            var targetItem = _model.TargetCell.ItemDescription;
            var targetAmount = _model.TargetCell.Amount;

            if (targetItem != null && targetItem.Id == sourceItem.Id)
            {
                var put = _model.TargetCell.TryPut(sourceItem, sourceAmount);
                _model.SourceCell.TryTake(put);
                return;
            }

            _model.SourceCell.TryTake(sourceAmount);
            _model.TargetCell.TryTake(targetAmount);
            _model.SourceCell.TryPut(targetItem, targetAmount);
            _model.TargetCell.TryPut(sourceItem, sourceAmount);
        }

        private void Move()
        {
            if (_model.TargetCell.ItemDescription != null && _model.CurrentItem != _model.TargetCell.ItemDescription)
            {
                return;
            }

            var remaining = _model.CurrentItem.StackSize - _model.TargetCell.Amount;
            var canPut = Math.Min(remaining, _model.CurrentAmount);

            _model.TargetCell.TryPut(_model.CurrentItem, canPut);
            _model.SourceCell.TryTake(canPut);
        }
    }
}