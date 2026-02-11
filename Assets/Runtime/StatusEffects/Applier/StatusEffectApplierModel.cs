using System;
using System.Linq;
using Runtime.Descriptions;
using Runtime.StatusEffects.Collection;

namespace Runtime.StatusEffects.Applier
{
    public class StatusEffectApplierModel
    {
        public event Func<string, string> OnApplyRequested;

        public StatusEffectModelCollection Collection { get; }

        public StatusEffectApplierModel(WorldDescription description)
        {
            Collection = new StatusEffectModelCollection(description.StatusEffectCollection);
        }

        public string TryApply(string descriptionKey)
        {
            return OnApplyRequested?.Invoke(descriptionKey);
        }

        public void RemoveById(string statusEffectId)
        {
            Collection.Remove(statusEffectId);
        }

        public void RemoveAllByDescriptionId(string descriptionKey)
        {
            var statusEffectModels = Collection.Models.Values.Where(model => model.Description.Id == descriptionKey)
                .ToList();

            foreach (var statusEffectModel in statusEffectModels)
            {
                Collection.Remove(statusEffectModel.Id);
            }
        }

        public bool HasStatusEffect(string descriptionKey)
        {
            return Collection.Models.Values.Any(statusEffectModel =>
                statusEffectModel.Description.Id == descriptionKey);
        }
    }
}