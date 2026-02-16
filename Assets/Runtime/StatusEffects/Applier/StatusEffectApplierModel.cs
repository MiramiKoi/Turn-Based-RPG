using System;
using System.Collections.Generic;
using System.Linq;
using Runtime.Descriptions;
using Runtime.ModelCollections;
using Runtime.StatusEffects.Collection;

namespace Runtime.StatusEffects.Applier
{
    public class StatusEffectApplierModel : ISerializable
    {
        public event Action OnApplyRequested;
        
        public Queue<string> ApplyQueue { get; } = new();
        public StatusEffectModelCollection Collection { get; }

        public StatusEffectApplierModel(WorldDescription description)
        {
            Collection = new StatusEffectModelCollection(description.StatusEffectCollection);
        }

        public void TryApply(string descriptionKey)
        {
            ApplyQueue.Enqueue(descriptionKey);
            OnApplyRequested?.Invoke();
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

        public Dictionary<string, object> Serialize()
        {
            return new Dictionary<string, object>();
        }
    }
}