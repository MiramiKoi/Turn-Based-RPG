using System.Collections.Generic;
using Runtime.Extensions;

namespace Runtime.Descriptions.CameraControl
{
    public class CameraControlDescription
    {
        public float CameraDragSensitivity { get; }

        public CameraControlDescription(Dictionary<string, object> data)
        {
            CameraDragSensitivity = data.GetFloat("drag_sensitivity");
        }
    }
}