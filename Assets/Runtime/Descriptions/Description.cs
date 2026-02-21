namespace Runtime.Descriptions
{
    public abstract class Description
    {
        public string Id { get; }

        protected Description(string id)
        {
            Id = id;
        }
    }
}