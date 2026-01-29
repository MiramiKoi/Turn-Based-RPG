using Runtime.Extensions;
using System.Collections.Generic;

namespace Runtime.Descriptions
{
    public sealed class WorldDescription
    {
        public SurfaceDescriptionCollection SurfaceCollection { get; private set; }

        public void SetData(Dictionary<string, object> data)
        {
            SurfaceCollection = new SurfaceDescriptionCollection(data.GetNode("surfaces"));
        }
    }
}