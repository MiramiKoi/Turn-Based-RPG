using System.Collections.Generic;
using System.Linq;
using Runtime.Descriptions.StatusEffects;
using Runtime.Descriptions.StatusEffects.Enums;
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

        public void Apply(string effectId)
        {
            var description = _descriptions.Get(effectId);

            if (description.Stacking.Mode == StackingMode.Independent)
            {
                Create(effectId);
                return;
            }

            var existing = Models.Values.FirstOrDefault(model => model.Description.Id == effectId);

            if (existing == null)
            {
                Create(effectId);
                return;
            }

            switch (description.Stacking.Mode)
            {
                case StackingMode.None:
                    return;

                case StackingMode.Refresh:
                case StackingMode.Additive:
                    existing.AddStack();
                    return;

                case StackingMode.Independent:
                default:
                    return;
            }
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