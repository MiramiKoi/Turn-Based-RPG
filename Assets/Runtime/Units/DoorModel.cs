using Runtime.Descriptions;
using Runtime.Descriptions.Units;
using UnityEngine;

namespace Runtime.Units
{
    public class DoorModel : UnitModel
    {
        public DoorModel(string id, Vector2Int position, UnitDescription description, WorldDescription worldDescription)
            : base(id, position, description, worldDescription)
        {
        }

        public Vector2Int ToPosition { get; set; }
    }
}