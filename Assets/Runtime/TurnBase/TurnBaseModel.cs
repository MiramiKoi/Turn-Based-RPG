using System;
using System.Collections.Generic;

namespace Runtime.TurnBase
{
    public class TurnBaseModel
    {
        public event Action OnPlayerStepFinished;
        public event Action OnWorldStepFinished;
        public event Action OnBuffTick;
        public event Action OnDebuffTick;
        public event Action OnMixedBuffTick;

        public int CurrentTurn { get; private set; }

        public Queue<StepModel> Steps { get; } = new();

        public void PlayerStep()
        {
            CurrentTurn++;
            OnPlayerStepFinished?.Invoke();
        }

        public void WorldStep()
        {
            OnWorldStepFinished?.Invoke();
            OnBuffTick?.Invoke();
            OnDebuffTick?.Invoke();
            OnMixedBuffTick?.Invoke();
        }
    }
}