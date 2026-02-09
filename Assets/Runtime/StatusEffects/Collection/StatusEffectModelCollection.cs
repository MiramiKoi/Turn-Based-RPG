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

        public void Create(StatusEffectDescription description)
        {
            CreateInternal(description, description.Id);
        }

        public override void Create(string effectId)
        {
            var description = _descriptions.Get(effectId);
            CreateInternal(description, effectId);
        }
        
        private void CreateInternal(StatusEffectDescription description, string effectId)
        {
            if (description.Stacking.Mode == StackingMode.Independent)
            {
                base.Create(effectId);
                return;
            }

            var existing = Models.Values
                .FirstOrDefault(model => model.Description.Id == effectId);

            if (existing == null)
            {
                base.Create(effectId);
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