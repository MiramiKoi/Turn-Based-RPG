using System;

namespace Runtime.TurnBase
{
    public class TurnBaseModel
    {
        public event Action OnPlayerStepFinished;
        public event Action OnWorldStepFinished;
        
        public int CurrentTurn { get; private set; }

        public void PlayerStep()
        {
            CurrentTurn++;
            OnPlayerStepFinished?.Invoke();
        }

        public void WorldStep()
        {
            OnWorldStepFinished?.Invoke();
        }
    }
}