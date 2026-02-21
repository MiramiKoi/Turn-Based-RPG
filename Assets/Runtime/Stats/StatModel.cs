using System;
using Runtime.Descriptions.Stats;

namespace Runtime.Stats
{
    public class StatModel
    {
        public event Action<float> ValueChanged;

        public StatDescription Description { get; }
        
        private float _value;
        private float _multiplier = 1.0f;
        public float Value
        {
            get => _multiplier * _value;
            private set => _value = value;
        }
        
        public StatModel(StatDescription description)
        {
            Description = description;
            Value = description.Value;
        }

        public void ChangeValue(float delta)
        {
            Value = Math.Max(0, Value + delta);
            ValueChanged?.Invoke(Value);
        }

        public void Set(float value)
        {
            Value = value;
            ValueChanged?.Invoke(Value);
        }

        public void Multiply(float factor)
        {
            _multiplier *= factor;
            ValueChanged?.Invoke(Value);
        }
    }
}