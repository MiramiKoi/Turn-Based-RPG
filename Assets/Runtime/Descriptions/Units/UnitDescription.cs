using System.Collections.Generic;
using Runtime.Descriptions.Stats;
using Runtime.Extensions;

namespace Runtime.Descriptions.Units
{
    public class UnitDescription : Description
    {
        public StatDescriptionCollection Stats { get; }
        public string Fraction { get; }
        public string ViewId { get; }
        public string AgentDecisionDescriptionId { get; }
        public int InventorySize { get; }
        public Dictionary<string, int> Loot { get; }
        public List<string> Equipment { get; }

        public UnitDescription(string id, Dictionary<string, object> data) : base(id)
        {
            Stats = new StatDescriptionCollection(data.GetNode("stats"));
            ViewId = data.GetString("view_id");
            Fraction = data.GetString("fraction");
            InventorySize = data.GetInt("inventory_size");
            Loot = data.GetDictionary<string, int>("loot");
            Equipment = data.GetList<string>("equipment");

            if (data.ContainsKey("agent_decision_id"))
            {
                AgentDecisionDescriptionId = data.GetString("agent_decision_id");
            }
        }
    }
}