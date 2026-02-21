using System.Collections.Generic;
using Runtime.Descriptions.Agents.Nodes;
using Runtime.Extensions;

namespace Runtime.Descriptions.Agents.Commands
{
    public class HasFlagCommand : CommandDescription
    {
        private const string FlagKey = "flag";
        private const string ValueKey = "value";

        public override string Type => "has_flag";

        public string Flag { get; private set; } = string.Empty;

        public bool Value { get; private set; }

        public override NodeStatus Execute(IWorldContext context, IControllable controllable)
        {
            if (!controllable.Flags.TryGetValue(Flag, out var flag))
            {
                return NodeStatus.Failure;
            }

            return flag == Value ? NodeStatus.Success : NodeStatus.Failure;
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