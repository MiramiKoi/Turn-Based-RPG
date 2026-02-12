using UnityEngine;

namespace Runtime.Units.Components
{
    public class UnitMovementModel
    {
        private readonly UnitStateModel _state;
        private readonly ActionBlockerModel _blocker;

        public UnitMovementModel(UnitStateModel state, ActionBlockerModel blocker)
        {
            _state = state;
            _blocker = blocker;
        }

        public void MoveTo(Vector2Int position)
        {
            if (!_blocker.CanExecute(UnitActionType.Move))
                return;

            var current = _state.Position.Value;

            if (position.x != current.x)
                Rotate(position.x < current.x ? UnitDirection.Left : UnitDirection.Right);

            _state.Position.Value = position;
        }

        public void Rotate(UnitDirection direction)
        {
            _state.Direction.Value = direction;
        }
    }
}