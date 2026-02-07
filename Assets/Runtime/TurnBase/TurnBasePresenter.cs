using System.Collections.Generic;
using System.Threading.Tasks;
using Runtime.AsyncLoad;
using Runtime.Common;
using Runtime.Core;
using Runtime.CustomAsync;

namespace Runtime.TurnBase
{
    public class TurnBasePresenter : IPresenter
    {
        private readonly TurnBaseModel _model;
        private readonly World _world;

        private readonly List<CustomAwaiter> _parallelAwaiters = new();

        public TurnBasePresenter(TurnBaseModel model, World world)
        {
            _model = model;
            _world = world;
        }

        public void Enable()
        {
            _model.OnPlayerStepFinished += OnPlayerMadeStep;
        }

        public void Disable()
        {
            _model.OnPlayerStepFinished -= OnPlayerMadeStep;
        }

        private async void OnPlayerMadeStep()
        {
            await ProcessAllSteps();
            
            foreach (var agent in _world.AgentCollection.Models.Values)
            {
                agent.MakeStep();
                await ProcessAllSteps();
            }
            
            await WaitParallelSteps();
            
            _model.WorldStep();
        }

        private async Task ProcessAllSteps()
        {
            while (_model.Steps.Count > 0)
            {
                var step = _model.Steps.Dequeue();
                await ProcessStep(step);
            }
        }
        
        private async Task ProcessStep(StepModel stepModel)
        {
            if (stepModel.StepType == StepType.Parallel)
            {
                stepModel.StepAction.Invoke();
                _parallelAwaiters.Add(stepModel.Awaiter);
            }
            else
            {
                stepModel.StepAction.Invoke();
                
                await WaitParallelSteps();
                await stepModel.Awaiter;
            }
        }
        
        private async Task WaitParallelSteps()
        {
            foreach (var awaiter in _parallelAwaiters)
            {
                await awaiter;
            }
            _parallelAwaiters.Clear();
        }
    }
}