using Runtime.ViewDescriptions.Landscape.Grid;
using Runtime.ViewDescriptions.Landscape.Surface;
using Runtime.ViewDescriptions.Unit;

namespace Runtime.ViewDescriptions
{
    public class WorldViewDescriptions
    {
        public SurfaceViewDescriptionCollection SurfaceViewDescriptions { get; set; }
        public GridIndicationViewDescription GridIndicationViewDescription { get; set; }
        public UnitViewDescriptionCollection UnitViewDescriptions { get; set; }
    }
}