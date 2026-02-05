using System.Collections.Generic;
using Runtime.Descriptions.Agents.Nodes;
using Runtime.Extensions;

namespace Runtime.Descriptions.Agents.Commands
{
    public class SetFlagCommand : CommandDescription
    {
        private const string FlagKey = "flag";
        private const string ValueKey = "value";

        public override string Type => "set_flag";

        public string Flag { get; private set; } = string.Empty;
        
        public bool Value { get; private set; } = false;
        
        public override NodeStatus Execute(IWorldContext context, IControllable controllable)
        {
            controllable.SetFlag(Flag, Value);
            
            return NodeStatus.Success;
        }

        public override Dictionary<string, object> Serialize()
        {
            var dictionary = base.Serialize();
            
            dictionary[FlagKey] = Flag;
            dictionary[ValueKey] = Value;
            
            return dictionary;
        }

        public override void Deserialize(Dictionary<string, object> dictionary)
        {
            Flag = dictionary.GetString(FlagKey);
            Value = dictionary.GetBool(ValueKey);
        }
    }
}