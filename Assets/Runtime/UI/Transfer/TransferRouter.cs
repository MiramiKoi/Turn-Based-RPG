using System.Collections.Generic;
using System.Linq;
using Runtime.UI.Inventory;
using Runtime.UI.Inventory.Cells;
using Runtime.UI.Transfer.Handlers;

namespace Runtime.UI.Transfer
{
    public class TransferRouter
    {
        private readonly List<ITransferHandler> _handlers = new();
        private readonly TransferModel _model;

        public TransferRouter(TransferModel model)
        {
            _model = model;
        }

        public void Register(ITransferHandler handler)
        {
            _handlers.Add(handler);
        }

        public void Unregister(ITransferHandler handler)
        {
            _handlers.Remove(handler);
        }

        private void Handle()
        {
            foreach (var handler in _handlers.Where(handler => handler.CanHandle(_model)))
            {
                handler.Handle(_model);
                return;
            }
        }

        public void HandleCellSelected(CellModel cell, InventoryModel inventory, InventoryType type)
        {
            if (_model.SourceCell == cell)
            {
                Clear();
                return;
            }

            if (_model.SourceCell == null)
            {
                FirstCellSelected(cell, inventory, type);
                return;
            }

            SecondCellSelected(cell, inventory, type);

            Clear();
        }
        
        private void FirstCellSelected(CellModel cell, InventoryModel inventory, InventoryType type)
        {
            if (cell.ItemDescription == null)
            {
                return;
            }

            _model.SourceCell = cell;
            _model.SourceInventory = inventory;
            _model.SourceType = type;
            cell.CellSelect();
        }
        
        private void SecondCellSelected(CellModel cell, InventoryModel inventory, InventoryType type)
        {
            _model.TargetCell = cell;
            _model.TargetInventory = inventory;
            _model.TargetType = type;
            
            Handle();
        }
        
        private void Clear()
        {
            _model.SourceCell.CellDeselect();
            _model.SourceCell = null;
            _model.SourceInventory = null;
            _model.SourceType = default;
            _model.TargetType = default;
            _model.TargetCell = null;
            _model.TargetInventory = null;
        }
    }
}