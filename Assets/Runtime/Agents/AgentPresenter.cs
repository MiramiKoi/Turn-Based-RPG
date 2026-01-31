using Runtime.Agents.Nodes;
using Runtime.Common;
using Runtime.Core;
using Runtime.TurnBase;
using Runtime.Units;

namespace Runtime.Agents
{
    public class AgentPresenter : IPresenter
    {
        private readonly UnitModel _unitModel;
        
        private readonly AgentDecisionRoot _decisionRoot;

        private readonly World _world;
        
        public AgentPresenter(UnitModel unitModel, AgentDecisionRoot decisionRoot, World world)
        {
            _unitModel = unitModel;
            _decisionRoot = decisionRoot;
            _world = world;
        }

        public void Enable()
        {
            _world.TurnBaseModel.OnStepFinished += HandleStep;
        }

        public void Disable()
        {
            _world.TurnBaseModel.OnStepFinished -= HandleStep;
        }

        private void HandleStep()
        {
            _decisionRoot.Process(_world, _unitModel);
        }
    }
}