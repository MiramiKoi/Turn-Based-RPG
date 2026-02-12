using System.Collections.Generic;
using Runtime.Descriptions;
using Runtime.Extensions;
using Runtime.ModelCollections;
using UnityEngine;

namespace Runtime.Units.Collection
{
    public class UnitModelCollection : DescribedModelCollection<UnitModel>
    {
        private readonly WorldDescription _descriptions;

        public UnitModelCollection(WorldDescription descriptions)
        {
            _descriptions = descriptions;
        }

        protected override UnitModel CreateModel(string descriptionKey)
        {
            var model = new UnitModel(GetCurrentKey(), Vector2Int.one, _descriptions.UnitCollection[descriptionKey], _descriptions);
            
            return model;
        }

        protected override UnitModel CreateModelFromData(string id, Dictionary<string, object> data)
        {
            var model = new UnitModel(id, data.GetVector2Int("position") ,_descriptions.UnitCollection[data.GetString("description_id")], _descriptions);

            return model;
        }
    }
}