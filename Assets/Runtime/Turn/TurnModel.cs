using System;

namespace Runtime.Turn
{
    public class TurnModel
    {
        public event Action OnInvoke;

        public int  CurrentTurn { get; private set; }
        
        public void Step()
        {
            CurrentTurn++;
            
            OnInvoke?.Invoke();
        }
    }
}