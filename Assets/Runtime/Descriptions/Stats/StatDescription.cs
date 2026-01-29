namespace Runtime.Descriptions.Stats
{
    public class StatDescription : Description
    {
        public float Value { get; }
        
        public StatDescription(string id, float value) : base(id)
        {
            Value = value;
        }
    }
}