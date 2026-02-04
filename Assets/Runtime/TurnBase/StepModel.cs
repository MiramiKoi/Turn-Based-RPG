using System;
using Runtime.AsyncLoad;

namespace Runtime.TurnBase
{
    public class StepModel
    {
        public StepType StepType { get; }
        
        public Action StepAction { get; }
        
        public CustomAwaiter Awaiter { get; }
        
        public StepModel(StepType stepType, Action stepAction, CustomAwaiter awaiter)
        {
            StepType = stepType;
            StepAction = stepAction;
            Awaiter = awaiter;
        }
    }
}