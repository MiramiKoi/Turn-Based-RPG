using System.Collections.Generic;
using Runtime.Descriptions.CameraControl;
using Runtime.Descriptions.Items;
using Runtime.Descriptions.Locations;
using Runtime.Descriptions.Locations.Environment;
using Runtime.Descriptions.Locations.Surface;
using Runtime.Descriptions.SpawnDirector;
using Runtime.Descriptions.StatusEffects;
using Runtime.Descriptions.Units;
using Runtime.Extensions;

namespace Runtime.Descriptions
{
    public sealed class WorldDescription
    {
        public CameraControlDescription CameraControlDescription { get; private set; }

        public SurfaceGenerationDescription SurfaceGenerationDescription { get; private set; }
        public SurfaceDescriptionCollection SurfaceCollection { get; private set; }
        public EnvironmentGenerationDescription EnvironmentGenerationDescription { get; private set; }
        public EnvironmentDescriptionCollection EnvironmentCollection { get; private set; }
        public LocationDescriptionCollection LocationCollection { get; private set; }

        public UnitDescriptionCollection UnitCollection { get; private set; }
        public AgentDecisionDescriptionCollection AgentDecisionDescriptionCollection { get; private set; }
        public SpawnDirectorDescription SpawnDirectorDescription { get; private set; }
        
        public ItemDescriptionCollection ItemCollection { get; private set; }

        public StatusEffectDescriptionCollection StatusEffectCollection { get; private set; }

        public void SetData(Dictionary<string, object> data)
        {
            CameraControlDescription = new CameraControlDescription(data.GetNode("camera_control"));

            SurfaceGenerationDescription = new SurfaceGenerationDescription(data.GetNode("surface_generation"));
            SurfaceCollection = new SurfaceDescriptionCollection(data.GetNode("surfaces"));
            EnvironmentGenerationDescription =
                new EnvironmentGenerationDescription(data.GetNode("environment_generation"),
                    data.GetNode("environment"));
            EnvironmentCollection = new EnvironmentDescriptionCollection(data.GetNode("environment"));
            LocationCollection = new LocationDescriptionCollection(data.GetNode("locations"));

            UnitCollection = new UnitDescriptionCollection(data.GetNode("units"));
            AgentDecisionDescriptionCollection = new AgentDecisionDescriptionCollection(data.GetNode("agent_decisions"));
            SpawnDirectorDescription = new SpawnDirectorDescription(data.GetNode("spawn_director"));

            ItemCollection = new ItemDescriptionCollection(data.GetNode("items"));

            StatusEffectCollection = new StatusEffectDescriptionCollection(data.GetNode("status_effects"));
        }
    }
}