using Runtime.Extensions;
using System.Collections.Generic;
using Runtime.Descriptions.Units;

namespace Runtime.Descriptions
{
    public sealed class WorldDescription
    {
        public SurfaceDescriptionCollection SurfaceCollection { get; private set; }
        public UnitDescriptionCollection UnitCollection { get; private set; }

        public void SetData(Dictionary<string, object> data)
        {
            SurfaceCollection = new SurfaceDescriptionCollection(data.GetNode("surfaces"));
            UnitCollection = new UnitDescriptionCollection(data.GetNode("units"));
        }
    }
}