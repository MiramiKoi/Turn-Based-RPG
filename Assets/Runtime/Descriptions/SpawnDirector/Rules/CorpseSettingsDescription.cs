using System.Collections.Generic;
using Runtime.Extensions;

namespace Runtime.Descriptions.SpawnDirector.Rules
{
    public class CorpseSettingsDescription
    {
        public int LifetimeSteps { get; }
        public bool IsInfinite => LifetimeSteps < 0;

        public CorpseSettingsDescription(Dictionary<string, object> data)
        {
            LifetimeSteps = data.GetInt("lifetime_steps");
        }
    }
}