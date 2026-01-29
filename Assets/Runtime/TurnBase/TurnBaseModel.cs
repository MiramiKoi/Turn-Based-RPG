using System;

namespace Runtime.TurnBase
{
    public class TurnBaseModel
    {
        public event Action OnStepFinished;

        public int CurrentTurn { get; private set; }

        public void Step()
        {
            CurrentTurn++;
            OnStepFinished?.Invoke();
        }
    }
}