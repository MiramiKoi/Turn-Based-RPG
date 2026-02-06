using Runtime.ViewDescriptions.Landscape.Environment;
using Runtime.ViewDescriptions.Landscape.Grid;
using Runtime.ViewDescriptions.Landscape.Surface;
using Runtime.ViewDescriptions.Units;

namespace Runtime.ViewDescriptions
{
    public class WorldViewDescriptions
    {
        public SurfaceViewDescriptionCollection SurfaceViewDescriptions { get; set; }
        public EnvironmentViewDescriptionCollection EnvironmentViewDescriptions { get; set; }
        public GridIndicationViewDescription GridIndicationViewDescription { get; set; }
        public UnitViewDescriptionCollection UnitViewDescriptions { get; set; }
    }
}