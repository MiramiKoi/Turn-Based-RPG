using System.Collections.Generic;
using UnityEngine;

namespace Runtime.ViewDescriptions.Unit
{
    [CreateAssetMenu(fileName = "UnitViewDescriptionCollection", menuName = "ViewDescription/Units/Unit Collection")]
    public class UnitViewDescriptionCollection : ScriptableObject
    {
        [SerializeField] private List<UnitViewDescription> _descriptions;
        
        public IReadOnlyList<UnitViewDescription> Descriptions => _descriptions;

        public UnitViewDescription Get(string id)
        {
            return _descriptions.Find(description => description.Id == id);
        }
    }
}