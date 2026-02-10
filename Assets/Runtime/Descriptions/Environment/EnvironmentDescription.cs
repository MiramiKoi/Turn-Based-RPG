using System.Collections.Generic;
using Runtime.Extensions;

namespace Runtime.Descriptions.Environment
{
    public class EnvironmentDescription : Description
    {
        public string ViewId { get; }
        public bool IsWalkable { get; }

        public bool BlockVision { get; }
        
        public EnvironmentDescription(string id, Dictionary<string, object> data) : base(id)
        {
            ViewId = data.GetString("view_id");
            IsWalkable = data.GetBool("is_walkable");
            BlockVision = data.GetBool("block_vision");
        }
    }
}