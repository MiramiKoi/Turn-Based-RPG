using System;

namespace Runtime.CustomAsync
{
    public class Scheduler 
    {
        public static Scheduler Instance { get; } = new();
        
        public event Action<float> OnTick;

        private Scheduler() { }
        
        public void Update(float deltaTime)
        {
            OnTick?.Invoke(deltaTime);
        }
    }
}