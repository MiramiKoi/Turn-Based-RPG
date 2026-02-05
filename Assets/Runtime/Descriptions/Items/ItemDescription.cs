using System.Collections.Generic;
using Runtime.Extensions;
using Runtime.UI.Inventory.Cells;

namespace Runtime.Descriptions.Items
{
    public class ItemDescription : IItemDescription
    {
        public string Id { get; }
        public string Type { get; }
        public int StackSize { get; }
        public string ViewId { get; }

        public ItemDescription(string id, Dictionary<string, object> data)
        {
            Id = id;
            Type = data.GetString("type");
            StackSize = data.GetInt("stack_size");
            ViewId = data.GetString("view_id");
        }
    }
}