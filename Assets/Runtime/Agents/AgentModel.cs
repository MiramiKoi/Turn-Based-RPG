using Runtime.Core;
using Runtime.Descriptions.Agents.Nodes;
using Runtime.Units;

namespace Runtime.Agents
{
    public class AgentModel
    {
        private readonly UnitModel _unitModel;

        private readonly AgentDecisionDescription _decisionDescription;

        private readonly World _world;

        public AgentModel(UnitModel unitModel, AgentDecisionDescription decisionDescription, World world)
        {
            _unitModel = unitModel;
            _decisionDescription = decisionDescription;
            _world = world;
        }

        public void MakeStep()
        {
            _decisionDescription.Process(_world, _unitModel);
        }
    }
}