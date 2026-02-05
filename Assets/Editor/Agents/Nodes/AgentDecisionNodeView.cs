using Runtime.Agents.Nodes;

namespace Editor.Agents.Nodes
{
    public class AgentDecisionNodeView : AgentBaseNodeView
    {
        public AgentDecisionNodeView(AgentDecisionRoot data) : base(data)
        {
            inputContainer.Clear();
        }
    }
}