namespace Runtime.ModelCollections
{
    public abstract class DescribedModelCollection<T> : SerializeModelCollection<T>
        where T : ISerializable
    {
        public void Create(string descriptionKey)
        {
            DescriptionKey = descriptionKey;
            var model = CreateModel(descriptionKey);
            Add(GetCurrentKey(), model);
        }
        
        protected abstract T CreateModel(string descriptionKey);
    }
}