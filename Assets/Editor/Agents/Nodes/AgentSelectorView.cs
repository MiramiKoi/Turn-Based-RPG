using Runtime.Descriptions.Agents.Nodes;

namespace Editor.Agents.Nodes
{
    public class AgentSelectorView : AgentBaseNodeView
    {
        public AgentSelectorView(AgentNodeEditorWrapper wrapper) : base(wrapper)
        {
        }

        public AgentSelectorView(AgentSelector data) : base(data)
        {
        }
    }
}