using System.Collections.Generic;
using Runtime.Extensions;

namespace Runtime.Descriptions.Items
{
    public class ItemDescription
    {
        public string Id { get; }
        public string Type { get; }
        public int StackSize { get; }
        public bool IsBuyable { get; }
        public int Price { get; }
        public string ViewId { get; }

        public ItemDescription(string id, Dictionary<string, object> data)
        {
            Id = id;
            Type = data.GetString("type");
            StackSize = data.GetInt("stack_size");
            IsBuyable = data.GetBool("is_buyable");
            Price = data.GetInt("price");
            ViewId = data.GetString("view_id");
        }
    }
}