using System.Collections.Generic;
using Runtime.Extensions;

namespace Runtime.Descriptions.CameraControl
{
    public class CameraControlDescription
    {
        public float DragSensitivity { get; }
        public float EdgeSize { get; }
        public float EdgeSensitivity { get; }

        public CameraControlDescription(Dictionary<string, object> data)
        {
            DragSensitivity = data.GetFloat("drag_sensitivity");
            EdgeSize = data.GetFloat("edge_size");
            EdgeSensitivity = data.GetFloat("edge_sensitivity");
        }
    }
}