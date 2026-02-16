using System.Collections.Generic;
using Runtime.Agents;
using Runtime.Descriptions.Agents.Nodes;
using Runtime.Units;
using UnityEngine;

namespace Runtime.Descriptions.Agents.Commands
{
    public class EnterToBattleCommand : CommandDescription
    {
        public override string Type => "enter_to_battle";

        public override NodeStatus Execute(IWorldContext context, IControllable controllable)
        {
            return NodeStatus.Success;
        }

        public override void Deserialize(Dictionary<string, object> data)
        {
        }
    }
}