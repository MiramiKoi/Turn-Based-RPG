using Runtime.Common;
using Runtime.Core;
using Runtime.Descriptions.Agents.Nodes;
using Runtime.Units;

namespace Runtime.Agents
{
    public class AgentPresenter : IPresenter
    {
        private readonly AgentDecisionDescription _decisionDescription;

        private readonly UnitModel _unitModel;

        private readonly World _world;

        public AgentPresenter(UnitModel unitModel, AgentDecisionDescription decisionDescription, World world)
        {
            _unitModel = unitModel;
            _decisionDescription = decisionDescription;
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
            _decisionDescription.Process(_world, _unitModel);
        }
    }
}