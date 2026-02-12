using System.Collections.Generic;
using Runtime.Core;
using Runtime.Descriptions;
using Runtime.Descriptions.Agents.Nodes;
using Runtime.Descriptions.Units;
using Runtime.Units;
using UnityEngine;

namespace Runtime.Agents
{
    public class AgentModel : UnitModel, IControllable
    {
        private readonly AgentDecisionDescription _decisionDescription;
        public IReadOnlyDictionary<string, Vector2Int> PointOfInterest => _pointOfInterest;
        private readonly Dictionary<string, Vector2Int> _pointOfInterest = new();

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

        public void SetPointOfInterest(string key, Vector2Int value)
        {
            _pointOfInterest[key] = value;
        }

        public Vector2Int GetPointOfInterest(string key)
        {
            return _pointOfInterest[key];
        }
    }
}