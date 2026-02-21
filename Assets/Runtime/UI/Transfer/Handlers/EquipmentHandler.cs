using Runtime.Descriptions.Items;
using Runtime.Units;

namespace Runtime.UI.Transfer.Handlers
{
    public class EquipmentHandler : BaseTransferHandler
    {
        private readonly UnitModel _unitModel;

        public EquipmentHandler(UnitModel unitModel)
        {
            _unitModel = unitModel;
        }

        public override bool CanHandle(TransferModel context)
        {
            return (context.SourceType == InventoryType.Equipment && context.TargetType == InventoryType.Player)
                   || (context.TargetType == InventoryType.Equipment && context.SourceType == InventoryType.Player);
        }

        public override void Handle(TransferModel context)
        {
            if (context.SourceInventory == context.TargetInventory)
            {
                base.Handle(context);
                return;
            }

            if (context.TargetType == InventoryType.Equipment)
            {
                var equipmentItem = context.SourceCell.ItemDescription as EquipmentItemDescription;

                if (equipmentItem == null)
                {
                    return;
                }

                var targetEquipmentItem = context.TargetCell.ItemDescription as EquipmentItemDescription;

                if (targetEquipmentItem != null &&
                    equipmentItem.EquipmentType != targetEquipmentItem.EquipmentType)
                {
                    return;
                }

                context.SourceCell.TryTake(1);
                _unitModel.Equipment.Change(equipmentItem, out var oldEquipment);

                if (oldEquipment != null)
                {
                    context.SourceCell.TryPut(oldEquipment, 1);
                }

                return;
            }

            if (context.SourceType == InventoryType.Equipment)
            {
                var equipmentItem = context.SourceCell.ItemDescription as EquipmentItemDescription;

                if (equipmentItem == null)
                {
                    return;
                }

                if (context.TargetCell.ItemDescription != null &&
                    context.TargetCell.ItemDescription.Id != equipmentItem.Id)
                {
                    return;
                }

                _unitModel.Equipment.Remove(equipmentItem.EquipmentType, out var removed);

                if (removed != null)
                {
                    context.TargetCell.TryPut(removed, 1);
                }
            }
        }
    }
}