using System.Collections.Generic;

namespace Runtime.Descriptions.Surface
{
    public class SurfaceDescriptionCollection
    {
        public readonly Dictionary<string, SurfaceDescription> Surfaces;

        public SurfaceDescriptionCollection(Dictionary<string, object> data)
        {
            Surfaces = new Dictionary<string, SurfaceDescription>();

            foreach (var description in data)
            {
                Surfaces.Add(description.Key,
                    new SurfaceDescription(description.Key, (Dictionary<string, object>)description.Value));
            }
        }
    }
}