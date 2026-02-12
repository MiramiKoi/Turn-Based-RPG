using Runtime.Common.Movement;
using Runtime.Descriptions;
using Runtime.Descriptions.Units;
using Runtime.Units;
using UnityEngine;

namespace Runtime.Player
{
    public class PlayerModel : UnitModel
    {
        public MovementQueueModel MovementQueueModel { get; } = new();

        public bool IsExecutingRoute { get; set; }

        public PlayerMode Mode { get; set; } = PlayerMode.Adventure;
        
        public PlayerModel(string id, Vector2Int position, UnitDescription description,
            WorldDescription worldDescription)
            : base(id, position, description, worldDescription)
        {
        }
    }
}