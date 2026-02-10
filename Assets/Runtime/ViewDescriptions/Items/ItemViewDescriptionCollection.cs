using System.Collections.Generic;
using UnityEngine;

namespace Runtime.ViewDescriptions.Items
{
    [CreateAssetMenu(fileName = "ItemViewDescriptionCollection",
        menuName = "ViewDescription/Items/ItemViewDescriptionCollection")]
    public class ItemViewDescriptionCollection : ScriptableObject
    {
        [SerializeField] private List<ItemViewDescription> _descriptions;

        public IReadOnlyList<ItemViewDescription> Descriptions => _descriptions;

        public ItemViewDescription Get(string id)
        {
            return _descriptions.Find(description => description.Id == id);
        }
    }
}