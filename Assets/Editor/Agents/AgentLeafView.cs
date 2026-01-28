using Runtime.Agents.Nodes;

namespace Editor.Agents
{
    public class AgentLeafView : AgentBaseNodeView
    {
        public AgentLeafView(AgentLeaf data) : base(data)
        {
            outputContainer.Clear();
        }
    }
}