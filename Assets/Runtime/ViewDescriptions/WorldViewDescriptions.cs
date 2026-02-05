using Runtime.ViewDescriptions.Inventory;
using Runtime.ViewDescriptions.Items;
using Runtime.ViewDescriptions.Landscape.Grid;
using Runtime.ViewDescriptions.Landscape.Surface;
using Runtime.ViewDescriptions.Units;

namespace Runtime.ViewDescriptions
{
    public class WorldViewDescriptions
    {
        public SurfaceViewDescriptionCollection SurfaceViewDescriptions { get; set; }
        public GridIndicationViewDescription GridIndicationViewDescription { get; set; }
        public UnitViewDescriptionCollection UnitViewDescriptions { get; set; }
        public InventoryViewDescription InventoryViewDescription { get; set; }
        public ItemViewDescriptionCollection ItemViewDescriptions { get; set; }
    }
}