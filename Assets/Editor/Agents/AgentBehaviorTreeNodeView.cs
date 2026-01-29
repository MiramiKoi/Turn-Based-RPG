using Runtime.Agents.Nodes;

namespace Editor.Agents
{
    public class AgentBehaviorTreeNodeView : AgentBaseNodeView
    {
        public AgentBehaviorTreeNodeView(AgentDecisionRoot data) : base(data)
        {
            inputContainer.Clear();
        }
    }
}