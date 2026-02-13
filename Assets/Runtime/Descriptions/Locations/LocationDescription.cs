using System;
using System.Collections.Generic;
using Runtime.Extensions;

namespace Runtime.Descriptions.Locations
{
    public class LocationDescription : Description
    {
        public int[] Surface { get; private set; }
        public int[] Environment { get; private set; }
        public string SurfaceGenerationRules { get; private set; }
        public string EnvironmentGenerationRules { get; private set; }

        public LocationDescription(string id, Dictionary<string, object> data) : base(id)
        {
            var surfaceList = data.GetList("surface");
            Surface = new int[surfaceList.Count];
            for (int i = 0; i < surfaceList.Count; i++)
            {
                Surface[i] = Convert.ToInt32(surfaceList[i]);
            }
            
            var environmentList = data.GetList("environment");
            Environment = new int[environmentList.Count];
            for (int i = 0; i < environmentList.Count; i++)
            {
                Environment[i] = Convert.ToInt32(environmentList[i]);
            }
            
            SurfaceGenerationRules = data.GetString("surface_generation_rules");
            EnvironmentGenerationRules = data.GetString("environment_generation_rules");
        }
    }
}