using Runtime.Agents.Nodes;
using Runtime.Units;
using UnityEngine;

namespace Runtime.Agents
{
    public class MoveCommand : IUnitCommand
    {
        private readonly Vector2Int _delta;

        public MoveCommand(Vector2Int delta)
        {
            _delta = delta;
        }

        public NodeStatus Execute(IWorldContext context, IControllable controllable)
        {
            var unit = controllable as UnitModel;

            var nextPosition = unit.Position.Value + _delta;
            
            var success = context.GridModel.TryPlace(unit, nextPosition);

            if (success)
            {
                context.GridModel.ReleaseCell(unit.Position.Value);
                unit.MoveTo(nextPosition);
            }
            
            return success ? NodeStatus.Success : NodeStatus.Failure;
        }
    }


    public class SetFlagCommand : IUnitCommand
    {
        private readonly string _key;
        
        private readonly bool _value;

        public SetFlagCommand(bool value, string key)
        {
            _value = value;
            _key = key;
        }

        public NodeStatus Execute(IWorldContext context, IControllable controllable)
        {
            var unit = controllable as UnitModel;

            unit?.SetFlag(_key, _value);
            
            return NodeStatus.Success;
        }
    }
}