using System.Collections.Generic;
using Runtime.Agents;
using Runtime.Core;
using Runtime.Descriptions;
using Runtime.Extensions;
using Runtime.ModelCollections;
using Runtime.Player;
using UnityEngine;

namespace Runtime.Units.Collection
{
    public class UnitModelCollection : DescribedModelCollection<UnitModel>
    {
        private readonly WorldDescription _descriptions;
        private readonly World _world;

        public UnitModelCollection(World world, WorldDescription descriptions)
        {
            _descriptions = descriptions;
            _world = world;
        }

        protected override UnitModel CreateModel(string descriptionKey)
        {
            if (descriptionKey == "door")
            {
                return new DoorModel(GetCurrentKey(), Vector2Int.one, _descriptions.UnitCollection[descriptionKey],
                    _descriptions);
            }

            UnitModel model = descriptionKey == "character"
                ? new PlayerModel(GetCurrentKey(), Vector2Int.one, _descriptions.UnitCollection[descriptionKey],
                    _descriptions)
                : new AgentModel(GetCurrentKey(), Vector2Int.one, _descriptions.UnitCollection[descriptionKey],
                    _descriptions, _world);

            return model;
        }

        protected override UnitModel CreateModelFromData(string id, Dictionary<string, object> data)
        {
            if (id == "door")
            {
                return new DoorModel(id, data.GetVector2Int("position"),
                    _descriptions.UnitCollection[data.GetString("description_id")], _descriptions);
            }
            
            UnitModel model = id == "character"
                ? new PlayerModel(id, data.GetVector2Int("position"),
                    _descriptions.UnitCollection[data.GetString("description_id")], _descriptions)
                : new AgentModel(id, data.GetVector2Int("position"),
                    _descriptions.UnitCollection[data.GetString("description_id")], _descriptions, _world);

            return model;
        }
    }
}