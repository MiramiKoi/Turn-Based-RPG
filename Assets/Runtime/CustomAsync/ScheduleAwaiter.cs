using System;
using System.Runtime.CompilerServices;

namespace Runtime.CustomAsync
{
    public class ScheduleAwaiter : INotifyCompletion
    {
        private Action _continuations;
        public bool IsCompleted { get; private set; }

        private float _awaitTime;
        private Scheduler _scheduler;

        public ScheduleAwaiter(float awaitTime, Scheduler scheduler)
        {
            _awaitTime = awaitTime;
            _scheduler = scheduler;
        }
        
        public void Start()
        {
            _scheduler.OnTick += OnTick;
        }

        public void Stop()
        {
            _scheduler.OnTick -= OnTick;
            
            IsCompleted = true;
            _continuations?.Invoke();
            _continuations = null;
        }
        
        private void OnTick(float deltaTime)
        {
            _awaitTime -= deltaTime;
            if (_awaitTime <= 0)
            {
                Stop();
            }
        }
        
        public void OnCompleted(Action continuation)
        {
            _continuations += continuation;
        }

        public void GetResult()
        {
        }

        public ScheduleAwaiter GetAwaiter() => this;
    }
}