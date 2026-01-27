using Runtime.Descriptions.Stats;

namespace Runtime.Stats
{
    public class StatModel
    {
        public StatDescription Description { get; }
        public float Value { get; }

        public StatModel(StatDescription description)
        {
            Description = description;
            Value = description.Value;
        }
    }
}