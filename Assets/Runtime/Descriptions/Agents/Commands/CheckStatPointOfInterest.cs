using System.Collections.Generic;
using Runtime.Descriptions.Agents.Nodes;
using Runtime.Extensions;
using Runtime.Units;
using UnityEngine;

namespace Runtime.Descriptions.Agents.Commands
{
    public class CheckStatPointOfInterest : CommandDescription
    {
        public override string Type => "check_stat_point_of_interest";
        
        private const string StatKey = "stat";
        
        private const string OperationKey = "operation";
        
        private const string ValueKey = "value";
        
        private const string PointOfInterestKey = "point_of_interest";

        public string Stat { get; private set; } = string.Empty;
        
        public string Operation { get; private set; } = string.Empty;
        
        public float Value { get; private set; }
        
        public string PointOfInterest { get; private set; } = string.Empty;
        
        public override NodeStatus Execute(IWorldContext context, IControllable controllable)
        {
            if (controllable.PointOfInterest.TryGetValue(PointOfInterest, out var position))
            {
                var cell = context.GridModel.GetCell(position);

                if (cell.Unit is not UnitModel unit)
                {
                    return NodeStatus.Failure;
                }
                
                if (unit.Stats.Stats.TryGetValue(StatKey, out var stat))
                {
                    return Operation switch
                    {
                        "=" => Mathf.Approximately(stat.Value, Value) ? NodeStatus.Success : NodeStatus.Failure,
                        ">" => stat.Value > Value ? NodeStatus.Success : NodeStatus.Failure,
                        "<" => stat.Value < Value ? NodeStatus.Success : NodeStatus.Failure,
                        "<=" => stat.Value <= Value ? NodeStatus.Success : NodeStatus.Failure,
                        ">=" => stat.Value >= Value ? NodeStatus.Success : NodeStatus.Failure,
                        _ => NodeStatus.Failure
                    };
                }
            }

            return NodeStatus.Failure;
        }

        public override Dictionary<string, object> Serialize()
        {
            var dictionary =  base.Serialize();
            
            dictionary[StatKey] = Stat;
            dictionary[OperationKey] = Operation;
            dictionary[ValueKey] = Value;
            dictionary[PointOfInterestKey] = PointOfInterest;
            
            return dictionary;
        }

        public override void Deserialize(Dictionary<string, object> data)
        {
            Stat = data.GetString(StatKey);
            Operation = data.GetString(OperationKey);
            Value = data.GetFloat(ValueKey);
            PointOfInterest = data.GetString(PointOfInterestKey);
        }
    }
}