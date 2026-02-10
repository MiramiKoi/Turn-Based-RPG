using System.Collections.Generic;
using UnityEngine;

namespace Runtime.ViewDescriptions.Landscape.Surface
{
    [CreateAssetMenu(fileName = "SurfaceViewDescriptionCollection",
        menuName = "ViewDescription/Landscape/SurfaceCollection")]
    public class SurfaceViewDescriptionCollection : ScriptableObject
    {
        [SerializeField] private List<SurfaceViewDescription> _descriptions;

        public SurfaceViewDescription Get(string id)
        {
            return _descriptions.Find(descriptions => descriptions.name == id);
        }
    }
}