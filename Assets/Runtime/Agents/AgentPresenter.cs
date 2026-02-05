using Runtime.Common;
using Runtime.Core;
using Runtime.Descriptions.Agents.Nodes;
using Runtime.Units;

namespace Editor.Agents
{
    public class AgentPresenter : IPresenter
    {
        private readonly AgentDecisionRoot _decisionRoot;
        
        private readonly UnitModel _unitModel;

        private readonly World _world;
        
        public AgentPresenter(UnitModel unitModel, AgentDecisionRoot decisionRoot, World world)
        {
            _unitModel = unitModel;
            _decisionRoot = decisionRoot;
            _world = world;
        }

        public void Enable()
        {
            _world.TurnBaseModel.OnPlayerStepFinished += OnStep;
        }

        public void Disable()
        {
            _world.TurnBaseModel.OnPlayerStepFinished -= OnStep;
        }

        private void OnStep()
        {
            _decisionRoot.Process(_world, _unitModel);
        }
    }
}