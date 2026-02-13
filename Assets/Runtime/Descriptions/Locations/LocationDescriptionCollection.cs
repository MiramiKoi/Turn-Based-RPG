using System.Collections.Generic;

namespace Runtime.Descriptions.Locations
{
    public class LocationDescriptionCollection
    {
        public readonly Dictionary<string, LocationDescription> Locations;

        public LocationDescriptionCollection(Dictionary<string, object> data)
        {
            Locations = new Dictionary<string, LocationDescription>();
            
            foreach (var description in data)
            {
                Locations.Add(description.Key, new LocationDescription(description.Key, (Dictionary<string, object>)description.Value));
            }
        }
    }
}
