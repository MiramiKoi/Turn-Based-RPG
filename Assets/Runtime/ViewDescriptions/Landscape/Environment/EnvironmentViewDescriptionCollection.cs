using System.Collections.Generic;
using UnityEngine;

namespace Runtime.ViewDescriptions.Landscape.Environment
{
    [CreateAssetMenu(fileName = "EnvironmentViewDescriptionCollection", menuName = "ViewDescription/Landscape/EnvironmentCollection")]
    public class EnvironmentViewDescriptionCollection : ScriptableObject
    {
        [SerializeField] private List<EnvironmentViewDescription> _descriptions;

        public EnvironmentViewDescription Get(string id)
        {
            return _descriptions.Find(descriptions => descriptions.name == id);
        }
    }
}
