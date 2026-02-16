using System;
using System.Collections.Generic;
using Runtime.TurnBase.Battle;

namespace Runtime.TurnBase
{
    public class TurnBaseModel
    {
        public event Action OnPlayerStepFinished;
        public event Action OnWorldStepFinished;
        public event Action OnBuffTick;
        public event Action OnDebuffTick;
        public event Action OnMixedBuffTick;
        public event Action OnBattleTick;

        public int CurrentTurn { get; private set; }

        public Queue<StepModel> Steps { get; } = new();

        public BattleModel BattleModel { get; } = new();

        public void PlayerStep()
        {
            CurrentTurn++;
            OnPlayerStepFinished?.Invoke();
        }

        public void WorldStep()
        {
            OnWorldStepFinished?.Invoke();
        }

        public void StatusEffectTick()
        {
            OnBuffTick?.Invoke();
            OnDebuffTick?.Invoke();
            OnMixedBuffTick?.Invoke();
        }

        public void BattleTick()
        {
            OnBattleTick?.Invoke();
        }
    }
}