using Runtime.ViewDescriptions.Landscape.Grid;
using Runtime.ViewDescriptions.Landscape.Surface;
using UnityEngine;

namespace Runtime.ViewDescriptions
{
    public class WorldViewDescriptions
    {
        public SurfaceViewDescriptionCollection SurfaceViewDescriptions { get; set; }
        public GridIndicationViewDescription GridIndicationViewDescription { get; set; }
    }
}