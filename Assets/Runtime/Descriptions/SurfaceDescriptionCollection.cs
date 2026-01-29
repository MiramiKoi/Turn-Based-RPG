using Runtime.Extensions;
using System.Collections.Generic;

namespace Runtime.Descriptions
{
    public class SurfaceDescriptionCollection
    {
        public Dictionary<string, SurfaceDescription> Surfaces;

        public SurfaceDescriptionCollection(Dictionary<string, object> data)
        {
            Surfaces = new Dictionary<string, SurfaceDescription>();

            foreach (var description in data)
            {
                Surfaces.Add(description.Key, new SurfaceDescription(description.Key, (Dictionary<string, object>)description.Value));
            }
        }
    }
}
