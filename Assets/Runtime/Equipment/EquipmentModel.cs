using System.Collections.Generic;
using Runtime.Descriptions.Items;
using Runtime.Descriptions.Stats;
using Runtime.UI.Inventory;

namespace Runtime.Equipment
{
    public class EquipmentModel
    {
        public InventoryModel Inventory { get; } = new(2);

        private readonly Dictionary<string, EquipmentItemDescription> _equipments = new();

        public void Add(EquipmentItemDescription description)
        {
            _equipments.Add(description.EquipmentType, description);
            Inventory.TryPutItem(description, 1);
        }

        public void Remove(string equipmentType, out EquipmentItemDescription oldEquipment)
        {
            _equipments.Remove(equipmentType, out oldEquipment);
            Inventory.TryTakeItem(oldEquipment, 1);
        }

        public void Change(EquipmentItemDescription description, out EquipmentItemDescription oldEquipment)
        {
            Remove(description.EquipmentType, out oldEquipment);
            Add(description);
        }

        public bool TryGetStats(string equipmentType, out StatDescriptionCollection stats)
        {
            stats = null;
            if (!_equipments.TryGetValue(equipmentType, out var equipment))
            {
                return false;
            }

            stats = equipment.Stats;
            return true;
        }
    }
}