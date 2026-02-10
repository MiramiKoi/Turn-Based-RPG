using System.Collections.Generic;

namespace Runtime.Descriptions.Environment
{
    public class EnvironmentDescriptionCollection
    {
        public readonly Dictionary<string, EnvironmentDescription> Environment;

        public EnvironmentDescriptionCollection(Dictionary<string, object> data)
        {
            Environment = new Dictionary<string, EnvironmentDescription>();

            foreach (var description in data)
            {
                Environment.Add(description.Key,
                    new EnvironmentDescription(description.Key, (Dictionary<string, object>)description.Value));
            }
        }
    }
}