using Runtime.Core;
using Runtime.Descriptions;
using Runtime.Descriptions.Agents.Nodes;
using Runtime.Descriptions.Units;
using Runtime.Units;
using UnityEngine;

namespace Runtime.Agents
{
    public class AgentModel : UnitModel
    {
        private readonly AgentDecisionDescription _decisionDescription;

        private readonly World _world;

        public AgentModel(string id, Vector2Int position, UnitDescription description,
            WorldDescription worldDescription, World world) : base(id,
            position, description, worldDescription)
        {
            _decisionDescription =
                worldDescription.AgentDecisionDescriptionCollection.Get(description.AgentDecisionDescriptionId);
            _world = world;
        }

        public void MakeStep()
        {
            _decisionDescription.Process(_world, this);
        }
    }
}