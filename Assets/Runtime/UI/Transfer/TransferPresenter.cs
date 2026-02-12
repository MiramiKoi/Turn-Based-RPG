using System;
using Runtime.Common;
using Runtime.UI.Inventory;
using Runtime.UI.Inventory.Cells;
using UniRx;

namespace Runtime.UI.Transfer
{
    public class TransferPresenter : IPresenter
    {
        protected readonly TransferModel _model;
        protected InventoryModel _currentSource;
        protected InventoryModel _currentTarget;

        private readonly CompositeDisposable _disposables = new();

        public TransferPresenter(TransferModel model)
        {
            _model = model;
        }

        public void Enable()
        {
            _model.SourceInventory.Subscribe(inventory => OnInventoryChanged(inventory, true)).AddTo(_disposables);
            _model.TargetInventory.Subscribe(inventory => OnInventoryChanged(inventory, false)).AddTo(_disposables);
        }

        public void Disable()
        {
            Unsubscribe(_currentSource, true);
            Unsubscribe(_currentTarget, false);

            _currentSource = null;
            _currentTarget = null;

            _model.SourceCell = null;
            _model.TargetCell = null;
            _disposables.Clear();
        }

        private void OnInventoryChanged(InventoryModel inventory, bool isSource)
        {
            if (isSource)
            {
                Unsubscribe(_currentSource, true);
                _currentSource = inventory;
                Subscribe(_currentSource, true);
            }
            else
            {
                Unsubscribe(_currentTarget, false);
                _currentTarget = inventory;
                Subscribe(_currentTarget, false);
            }
        }

        private void Subscribe(InventoryModel inventory, bool isSource)
        {
            if (inventory == null)
            {
                return;
            }

            if (isSource)
            {
                inventory.OnCellSelected += HandleSourceCellSelected;
            }
            else
            {
                inventory.OnCellSelected += HandleTargetCellSelected;
            }
        }

        private void Unsubscribe(InventoryModel inventory, bool isSource)
        {
            if (inventory == null)
            {
                return;
            }

            if (isSource)
            {
                inventory.OnCellSelected -= HandleSourceCellSelected;
            }
            else
            {
                inventory.OnCellSelected -= HandleTargetCellSelected;
            }
        }

        private void HandleSourceCellSelected(CellModel cell)
        {
            HandleCellSelected(cell, true);
        }

        private void HandleTargetCellSelected(CellModel cell)
        {
            HandleCellSelected(cell, false);
        }

        private void HandleCellSelected(CellModel cell, bool isSource)
        {
            if (_model.SourceCell == null)
            {
                if (cell.ItemDescription == null)
                {
                    return;
                }

                _model.SourceCell = cell;
                cell.CellSelect();

                return;
            }

            _model.TargetCell = cell;
            _model.SourceCell?.CellDeselect();
            
            Execute(isSource);

            _model.SourceCell = null;
            _model.TargetCell = null;
        }

        protected virtual void Execute(bool targetIsSource)
        {
            if (_model.SourceCell == _model.TargetCell)
            {
                return;
            }

            if (targetIsSource)
            {
                Swap();
            }
            else
            {
                Move();
            }
        }

        private void Swap()
        {
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
            if (_model.TargetCell.ItemDescription != null &&
                _model.TargetCell.ItemDescription.Id != _model.SourceCell.ItemDescription.Id)
            {
                return;
            }

            var remaining = _model.SourceCell.ItemDescription.StackSize - _model.TargetCell.Amount;
            var canPut = Math.Min(remaining, _model.SourceCell.Amount);

            _model.TargetCell.TryPut(_model.SourceCell.ItemDescription, canPut);
            _model.SourceCell.TryTake(canPut);
        }
    }
}