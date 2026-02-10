using System.Linq;

namespace Runtime.Descriptions.Agents.Nodes
{
    public class AgentInverse : AgentNode
    {
        public override string Type => "inverse";
        public override NodeStatus Process(IWorldContext context, IControllable controllable)
        {
            var child = Children.First();
            
            return child.Process(context, controllable) == NodeStatus.Success ?  NodeStatus.Failure : NodeStatus.Success;;
        }
    }
}