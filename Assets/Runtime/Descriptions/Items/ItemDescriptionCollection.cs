using System.Collections.Generic;

namespace Runtime.Descriptions.Items
{
    public class ItemDescriptionCollection
    {
        public Dictionary<string, ItemDescription> Descriptions { get; }

        public ItemDescriptionCollection(Dictionary<string, object> data)
        {
            Descriptions = new Dictionary<string, ItemDescription>();

            foreach (var (id, description) in data)
            {
                var itemData = (Dictionary<string, object>)description;
                Descriptions.Add(id, new ItemDescription(id, itemData));
            }
        }
    }
}