using System.Collections.Generic;
using Runtime.Descriptions.Items;
using Runtime.Descriptions.Stats;

namespace Runtime.Equipment
{
    public class EquipmentModel
    {
        private readonly Dictionary<string, EquipmentItemDescription> _equipments = new();

        public void Add(EquipmentItemDescription description)
        {
            _equipments.Add(description.EquipmentType, description);
        }

        public void Remove(string equipmentType, out EquipmentItemDescription oldEquipment)
        {
            _equipments.Remove(equipmentType, out oldEquipment);
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