using Runtime.Agents.Nodes;

namespace Editor.Agents
{
    public class AgentBehaviorTreeNodeView : AgentBaseNodeView
    {
        public AgentBehaviorTreeNodeView(AgentBehaviorTree data) : base(data)
        {
            inputContainer.Clear();
        }
    }
}