using Runtime.Extensions;
using System.Collections.Generic;
using Runtime.Descriptions.Surface;
using Runtime.Descriptions.Units;

namespace Runtime.Descriptions
{
    public sealed class WorldDescription
    {
        public SurfaceGenerationDescription SurfaceGenerationDescription { get; private set; }
        public SurfaceDescriptionCollection SurfaceCollection { get; private set; }
        public UnitDescriptionCollection UnitCollection { get; private set; }

        public void SetData(Dictionary<string, object> data)
        {
            SurfaceGenerationDescription = new SurfaceGenerationDescription(data.GetNode("surface_generation"));
            SurfaceCollection = new SurfaceDescriptionCollection(data.GetNode("surfaces"));
            UnitCollection = new UnitDescriptionCollection(data.GetNode("units"));
        }
    }
}