using System.Collections.Generic;
using Runtime.Descriptions.Items;
using Runtime.Descriptions.Stats;
using Runtime.Stats;
using Runtime.UI.Inventory;

namespace Runtime.Equipment
{
    public class EquipmentModel
    {
        public InventoryModel Inventory { get; } = new(2);

        private readonly Dictionary<string, EquipmentItemDescription> _equipments = new();

        private readonly StatModelCollection _unitStats;

        public EquipmentModel(StatModelCollection unitStats)
        {
            _unitStats = unitStats;
        }

        public void Add(EquipmentItemDescription description)
        {
            _equipments.Add(description.EquipmentType, description);
            Inventory.TryPutItem(description, 1);

            if (description.EquipmentType == "weapon")
            {
                SetStats(description.Stats);
            }
            else
            {
                ChangeStats(description.Stats, 1);
            }
        }

        public void Remove(string equipmentType, out EquipmentItemDescription oldEquipment)
        {
            _equipments.Remove(equipmentType, out oldEquipment);
            Inventory.TryTakeItem(oldEquipment, 1);

            if (oldEquipment == null) 
                return;
            
            if (oldEquipment.EquipmentType == "weapon")
            {
                UnsetStats(oldEquipment.Stats);
            }
            else
            {
                ChangeStats(oldEquipment.Stats, -1);
            }
        }

        public void Change(EquipmentItemDescription description, out EquipmentItemDescription oldEquipment)
        {
            Remove(description.EquipmentType, out oldEquipment);
            Add(description);
        }
        
        private void ChangeStats(StatDescriptionCollection stats, int multiplayer)
        {
            foreach (var stat in stats)
            {
                _unitStats[stat.Id].ChangeValue(stat.Value * multiplayer);
            }
        }

        private void SetStats(StatDescriptionCollection stats)
        {
            foreach (var stat in stats)
            {
                _unitStats[stat.Id].Set(stat.Value);
            }
        }

        private void UnsetStats(StatDescriptionCollection stats)
        {
            foreach (var stat in stats)
            {
                _unitStats[stat.Id].Set(_unitStats[stat.Id].Description.Value);
            }
        }
    }
}