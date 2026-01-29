using System;

namespace Runtime.TurnBase
{
    public class TurnBaseModel
    {
        public event Action OnInvoke;

        public int CurrentTurn { get; private set; }

        public void Step()
        {
            CurrentTurn++;

            OnInvoke?.Invoke();
        }
    }
}