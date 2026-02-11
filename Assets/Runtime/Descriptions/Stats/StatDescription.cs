namespace Runtime.Descriptions.Stats
{
    public class StatDescription : Description
    {
        public float MaxValue { get; }

        public StatDescription(string id, float maxValue) : base(id)
        {
            MaxValue = maxValue;
        }
    }
}