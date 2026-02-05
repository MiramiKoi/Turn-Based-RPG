using Runtime.Agents.Nodes;

namespace Editor.Agents.NodeViews
{
    public class AgentDecisionNodeView : AgentBaseNodeView
    {
        public AgentDecisionNodeView(AgentDecisionRoot data) : base(data)
        {
            inputContainer.Clear();
        }
    }
}