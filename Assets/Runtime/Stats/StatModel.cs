using System;
using Runtime.Descriptions.Stats;

namespace Runtime.Stats
{
    public class StatModel
    {
        public event Action<float> ValueChanged;
        
        public StatDescription Description { get; }
        public float Value { get; private set; }

        public StatModel(StatDescription description)
        {
            Description = description;
            Value = description.Value;
        }

        public void ChangeValue(float delta)
        {
            Value += delta;
            ValueChanged?.Invoke(Value);
        }
    }
}