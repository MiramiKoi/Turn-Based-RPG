using Runtime.Units.Rotation;
using UniRx;
using UnityEngine;

namespace Runtime.Units
{
    public class UnitStateModel
    {
        public ReactiveProperty<Vector2Int> Position { get; }
        public ReactiveProperty<UnitDirection> Direction { get; }
        public ReactiveProperty<bool> Visible { get; } = new(true);

        public UnitStateModel(Vector2Int startPosition)
        {
            Position = new ReactiveProperty<Vector2Int>(startPosition);
            Direction = new ReactiveProperty<UnitDirection>();
        }
    }
}