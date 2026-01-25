using System.Collections.Generic;
using UnityEngine;

namespace Runtime.ViewDescriptions.Landscape.Surface
{
    [CreateAssetMenu(fileName = "SurfaceViewDescriptionCollection", menuName = "ViewDescription/Landscape/SurfaceCollection")]
    public class SurfaceViewDescriptionCollection : ScriptableObject
    {
        public List<SurfaceViewDescription> Descriptions;
    }
}