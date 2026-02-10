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

        public UnitDescription(string id, Dictionary<string, object> data) : base(id)
        {
            Stats = new StatDescriptionCollection(data.GetNode("stats"));
            ViewId = data.GetString("view_id");
            Fraction = data.GetString("fraction");
        }
    }
}