using System.Collections.Generic;
using Runtime.Extensions;
using UnityEngine;

namespace Runtime.Descriptions.SpawnDirector.Rules
{
    public class SpawnSettingsDescription
    {
        public string Mode { get; }
        public Vector2Int? FixedPosition { get; }

        public SpawnSettingsDescription(Dictionary<string, object> data)
        {
            Mode = data.GetString("mode");

            if (Mode == "fixed")
            {
                FixedPosition = data.GetVector2Int("position");
            }
        }
    }
}