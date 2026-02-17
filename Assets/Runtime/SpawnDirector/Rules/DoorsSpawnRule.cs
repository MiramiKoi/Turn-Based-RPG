using System.Collections.Generic;
using System.Linq;
using Runtime.Core;
using Runtime.Landscape.Grid;
using Runtime.Units;
using UnityEngine;

namespace Runtime.SpawnDirector.Rules
{
    public class DoorsSpawnRule : SpawnRulePresenter
    {
        private readonly Dictionary<string, LocationModel> _gridModelLocationModelCollection;

        public DoorsSpawnRule(SpawnRuleModel ruleModel, World world) : base(ruleModel, world)
        {
            _gridModelLocationModelCollection = world.GridModel.LocationModelCollection;
        }

        protected override void Initialize()
        {
            foreach (var locationModel in _gridModelLocationModelCollection.Values)
            {
                if (locationModel.LocationDescription.Entrance != null && locationModel.LocationDescription.Exit != null)
                {
                    var Entrance = SpawnDoor(locationModel.Entrance, locationModel.Exit);

                    SpawnDoor(locationModel.Exit, Entrance);
                }
            }
        }

        protected override void HandleStep()
        {
        }

        private Vector2Int SpawnDoor(Vector2Int? spawnPosition, Vector2Int? toPosition)
        {
            var door = (DoorModel)World.UnitCollection.Create(Model.Description.UnitDescriptionId);
            if (spawnPosition.HasValue)
            {
                door.Movement.SetPosition(spawnPosition.Value);
            }
            else
            {
                door.Movement.SetPosition(World.GridModel.GetRandomAvailablePosition() ?? Vector2Int.zero);
            }

            if (toPosition.HasValue)
            {
                door.ToPosition = World.GridModel.GetNeighborAvailablePositions(toPosition.Value).First();
            }
            else
            {
                door.ToPosition = World.GridModel.GetRandomAvailablePosition() ?? Vector2Int.zero;
            }

            Model.Units.Add(door);
            return door.State.Position.Value;
        }
    }
}