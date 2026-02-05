using Runtime.Agents.Nodes;

namespace Editor.Agents.Nodes
{
    public class AgentSequenceView : AgentBaseNodeView
    {
        public AgentSequenceView(AgentNodeEditorWrapper wrapper) : base(wrapper)
        {
            
        }
        
        public AgentSequenceView(AgentSequence data) : base(data)
        {
        }
    }
}