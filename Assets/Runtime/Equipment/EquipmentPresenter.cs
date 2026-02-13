using Runtime.Common;
using Runtime.Core;
using Runtime.Descriptions.Items;
using Runtime.UI.Inventory.Cells;
using Runtime.Units;

namespace Runtime.Equipment
{
    public class EquipmentPresenter : IPresenter
    {
        private readonly UnitModel _unitModel;
        private readonly World _world;

        public EquipmentPresenter(UnitModel unitModel, World world)
        {
            _unitModel = unitModel;
            _world = world;
        }

        public void Enable()
        {
            _unitModel.Equipment.Inventory.OnCellSelected += OnCellSelected;
        }
        
        public void Disable()
        {
            _unitModel.Equipment.Inventory.OnCellSelected -= OnCellSelected;
        }
        
        private void OnCellSelected(CellModel cell)
        {
            var sourceCell = _world.TransferModel.SourceCell;
            if (sourceCell == null || !_unitModel.Inventory.Cells.Contains(sourceCell))
            {
                return;
            }

            var sourceItem = sourceCell.ItemDescription;
            var targetEquipmentItem = (EquipmentItemDescription)cell.ItemDescription;
            if (sourceItem is EquipmentItemDescription equipmentItem
                && equipmentItem.EquipmentType == targetEquipmentItem.EquipmentType)
            {
                sourceCell.TryTake(1);
                _unitModel.Equipment.Change(equipmentItem, out var oldEquipment);
                sourceCell.TryPut(oldEquipment, 1);
                
                sourceCell.CellDeselect();
                _world.TransferModel.SourceCell = null;
            }
        }
    }
}