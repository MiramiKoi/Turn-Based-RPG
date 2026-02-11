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
            Value = description.MaxValue;
        }

        public void ChangeValue(float delta)
        {
            Value = Math.Clamp(Value + delta, 0, Description.MaxValue);
            ValueChanged?.Invoke(Value);
        }

        public void Set(float value)
        {
            Value = value;
            ValueChanged?.Invoke(Value);
        }

        public void Multiply(float factor)
        {
            Value *= factor;
            ValueChanged?.Invoke(Value);
        }
    }
}