using System.Collections.Generic;
using Runtime.Extensions;

namespace Runtime.Descriptions.Locations.Surface
{
    public class SurfaceDescription : Description
    {
        public string ViewId { get; }
        public bool IsWalkable { get; }

        public SurfaceDescription(string id, Dictionary<string, object> data) : base(id)
        {
            ViewId = data.GetString("view_id");
            IsWalkable = data.GetBool("is_walkable");
        }
    }
}