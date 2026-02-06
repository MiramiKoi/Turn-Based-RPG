using Runtime.Descriptions.Items;

namespace Runtime.Items
{
    public class ItemFactory
    {
        private readonly ItemDescriptionCollection _collection;

        public ItemFactory(ItemDescriptionCollection collection)
        {
            _collection = collection;
        }

        public ItemModel Create(string id)
        {
            return new ItemModel(_collection.Descriptions[id]);
        }
    }
}