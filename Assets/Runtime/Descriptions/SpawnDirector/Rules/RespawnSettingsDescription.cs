using System.Collections.Generic;
using Runtime.Extensions;

namespace Runtime.Descriptions.SpawnDirector.Rules
{
    public class RespawnSettingsDescription
    {
        public bool Enabled { get; }
        public int DelaySteps { get; }

        public RespawnSettingsDescription(Dictionary<string, object> data)
        {
            Enabled = data.GetBool("enabled");
            if (Enabled)
                DelaySteps = data.GetInt("delay_steps");
        }
    }
}