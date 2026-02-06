using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UIElements;

namespace Runtime.ViewDescriptions.StatusEffects
{
    [CreateAssetMenu(fileName = "StatusEffectViewDescriptionCollection", menuName = "ViewDescription/StatusEffects/StatusEffect Collection")]
    public class StatusEffectViewDescriptionCollection : ScriptableObject
    {
        public AssetReferenceT<VisualTreeAsset> StatusEffectContainerAsset;
        public IReadOnlyList<StatusEffectViewDescription> Descriptions => _descriptions;

        [SerializeField] private List<StatusEffectViewDescription> _descriptions;

        public StatusEffectViewDescription Get(string id)
        {
            return _descriptions.Find(description => description.Id == id);
        }
    }
}
