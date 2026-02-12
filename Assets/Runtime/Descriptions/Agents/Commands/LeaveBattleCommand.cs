using System.Collections.Generic;
using Runtime.Descriptions.Agents.Nodes;
using Runtime.Units;

namespace Runtime.Descriptions.Agents.Commands
{
    public class LeaveBattleCommand : CommandDescription
    {
        public override string Type => "leave_battle";
        public override NodeStatus Execute(IWorldContext context, IControllable controllable)
        {
            context.TurnBaseModel.BattleModel.LeaveBattle((UnitModel)controllable);
            
            return NodeStatus.Success;
        }

        public override void Deserialize(Dictionary<string, object> data)
        {
            
        }
    }
}