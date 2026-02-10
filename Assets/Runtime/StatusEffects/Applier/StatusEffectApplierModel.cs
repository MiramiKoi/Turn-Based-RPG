using System;
using System.Linq;
using Runtime.Descriptions;
using Runtime.StatusEffects.Collection;

namespace Runtime.StatusEffects.Applier
{
    public class StatusEffectApplierModel
    {
        public event Func<string, string> OnApplyRequested;
        public event Action<string> OnRemoveRequested;

        public StatusEffectModelCollection Collection { get; }

        public StatusEffectApplierModel(WorldDescription description)
        {
            Collection = new StatusEffectModelCollection(description.StatusEffectCollection);
        }

        public string TryApply(string descriptionKey)
        {
            return OnApplyRequested?.Invoke(descriptionKey);
        }

        public void TryRemove(string statusEffectId)
        {
            OnRemoveRequested?.Invoke(statusEffectId);
        }

        public bool HasStatusEffect(string descriptionKey)
        {
            return Collection.Models.Values.Any(statusEffectModel => statusEffectModel.Description.Id == descriptionKey);
        }
    }
}