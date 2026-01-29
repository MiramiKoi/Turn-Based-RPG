using System;
using System.Threading.Tasks;

namespace Runtime.TurnBase
{
    public class TurnBaseModel
    {
        public event Action OnStepFinished;

        public int CurrentTurn { get; private set; }

        public async void Step()
        {
            CurrentTurn++;
            await Task.Delay(100);
            OnStepFinished?.Invoke();
        }
    }
}