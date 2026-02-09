using System;
using Runtime.Descriptions;
using Runtime.StatusEffects.Collection;

namespace Runtime.StatusEffects.Applier
{
    public class StatusEffectApplierModel
    {
        public event Action<string> OnApplyRequested;
        public event Action<string> OnRemoveRequested;

        public StatusEffectModelCollection Collection { get; }

        public StatusEffectApplierModel(WorldDescription description)
        {
            Collection = new StatusEffectModelCollection(description.StatusEffectCollection);
        }

        public void TryApply(string descriptionKey)
        {
            OnApplyRequested?.Invoke(descriptionKey);
        }

        public void TryRemove(string descriptionKey)
        {
            OnRemoveRequested?.Invoke(descriptionKey);
        }
    }
}