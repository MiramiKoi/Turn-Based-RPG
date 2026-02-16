using System.Collections.Generic;
using Runtime.Agents;
using Runtime.Descriptions.Agents.Nodes;
using Runtime.Extensions;
using UnityEngine;

namespace Runtime.Descriptions.Agents.Commands
{
    public class CheckStatPercentCommand : CommandDescription
    {
        private const string StatKey = "stat";
        private const string OperationKey = "operation";
        private const string ValueKey = "value";
        
        public override string Type => "check_stat_percent";

        public string Stat { get; private set; } = string.Empty;
        
        public string Operation { get; private set; } = string.Empty;
        
        public float Value { get; private set; }
        
        public override NodeStatus Execute(IWorldContext context, IControllable controllable)
        {
            if (!controllable.Stats.Stats.TryGetValue(Stat, out var statModel))
            {
                return NodeStatus.Failure;
            }
            
            var agentModel = controllable as AgentModel;
            
            var statValue = statModel.Value / agentModel.Description.Stats[Stat].Value;

            return Operation switch
            {
                "=" => Mathf.Approximately(statValue, Value) ? NodeStatus.Success : NodeStatus.Failure,
                ">" => statValue > Value ? NodeStatus.Success : NodeStatus.Failure,
                "<" => statValue < Value ? NodeStatus.Success : NodeStatus.Failure,
                "<=" => statValue <= Value ? NodeStatus.Success : NodeStatus.Failure,
                ">=" => statValue >= Value ? NodeStatus.Success : NodeStatus.Failure,
                _ => NodeStatus.Failure
            };
        }

        public override Dictionary<string, object> Serialize()
        {
            var dictionary =  base.Serialize();
            
            dictionary[StatKey] = Stat;
            dictionary[OperationKey] = Operation;
            dictionary[ValueKey] = Value;
            
            return dictionary;
        }

        public override void Deserialize(Dictionary<string, object> data)
        {
            Stat = data.GetString(StatKey);
            Operation = data.GetString(OperationKey);
            Value = data.GetFloat(ValueKey);
        }
    }
}