using Runtime.Descriptions.Items;

namespace Runtime.Items
{
    public class ItemModel
    {
        public ItemDescription Description;

        public ItemModel(ItemDescription description)
        {
            Description = description;
        }
    }
}