using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UIElements;

namespace Runtime.ViewDescriptions.StatusEffects
{
    [CreateAssetMenu(fileName = "StatusEffectViewDescriptionCollection", menuName = "ViewDescription/StatusEffects/StatusEffect Collection")]
    public class StatusEffectViewDescriptionCollection : ScriptableObject
    {
        [SerializeField] private List<StatusEffectViewDescription> _descriptions;
        [SerializeField] private AssetReferenceT<VisualTreeAsset> _statusEffectContainerAsset;

        public IReadOnlyList<StatusEffectViewDescription> Descriptions => _descriptions;

        public StatusEffectViewDescription Get(string id)
        {
            return _descriptions.Find(description => description.Id == id);
        }
    }
}
