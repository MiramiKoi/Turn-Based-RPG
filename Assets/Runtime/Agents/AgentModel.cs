using Runtime.Agents.Nodes;
using Runtime.Core;
using Runtime.Units;

namespace Runtime.Agents
{
    public class AgentModel
    {
        private readonly UnitModel _unitModel;
        
        private readonly AgentDecisionRoot _decisionRoot;

        private readonly World _world;
        
        public AgentModel(UnitModel unitModel, AgentDecisionRoot decisionRoot, World world)
        {
            _unitModel = unitModel;
            _decisionRoot = decisionRoot;
            _world = world;
        }

        public void MakeStep()
        {
            _decisionRoot.Process(_world, _unitModel);
        }
    }
}