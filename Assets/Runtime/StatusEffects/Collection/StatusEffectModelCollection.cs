using System.Collections.Generic;
using Runtime.Descriptions.StatusEffects;
using Runtime.ModelCollections;

namespace Runtime.StatusEffects.Collection
{
    public class StatusEffectModelCollection : DescribedModelCollection<StatusEffectModel>
    {
        private readonly StatusEffectDescriptionCollection _descriptions;

        public StatusEffectModelCollection(StatusEffectDescriptionCollection descriptions)
        {
            _descriptions = descriptions;
        }

        protected override StatusEffectModel CreateModelFromData(string id, Dictionary<string, object> data)
        {
            var description = _descriptions.Get(id);

            var model = new StatusEffectModel(id, description);
            
            model.Deserialize(data);
            return model;
        }

        protected override StatusEffectModel CreateModel(string descriptionKey)
        {
            DescriptionKey = descriptionKey;
            var description = _descriptions.Get(descriptionKey);
            return new StatusEffectModel(GetCurrentKey(), description);
        }
    }
}
