using System;

namespace Runtime.CustomAsync
{
    public class Scheduler 
    {
        public event Action<float> OnTick;

        public void Update(float deltaTime)
        {
            OnTick?.Invoke(deltaTime);
        }
    }
}