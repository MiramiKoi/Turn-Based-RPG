using System.Collections.Generic;
using Runtime.Extensions;
using UnityEngine;

namespace Runtime.Descriptions.Locations
{
    public class ExitDescription
    {
        public string Type { get; }
        public Vector2Int? Position { get; }
        
        public ExitDescription(Dictionary<string, object> data)
        {
            Type = data.GetString("type");
            
            if (data.ContainsKey("position"))
            {
                Position = data.GetVector2Int("position");
            }
        }
    }
}