using System.Collections.Generic;
using Runtime.Descriptions.Stats;
using Runtime.Extensions;

namespace Runtime.Descriptions.Items
{
    public class EquipmentItemDescription : ItemDescription
    {
        public string EquipmentType { get; }
        
        public StatDescriptionCollection Stats { get; }
        
        public EquipmentItemDescription(string id, Dictionary<string, object> data) : base(id, data)
        {
            EquipmentType = data.GetString("equipment_type");
            Stats = new StatDescriptionCollection(data.GetNode("stats"));
        }
    }
}