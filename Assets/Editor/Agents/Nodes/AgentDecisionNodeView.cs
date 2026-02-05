using Runtime.Agents.Nodes;

namespace Editor.Agents.Nodes
{
    public class AgentDecisionNodeView : AgentBaseNodeView
    {
        public AgentDecisionNodeView(AgentNodeEditorWrapper wrapper) :  base(wrapper)
        {
            
        }
        
        public AgentDecisionNodeView(AgentDecisionRoot data) : base(data)
        {
        }

        protected override void Setup()
        {
            base.Setup();
            
            inputContainer.Clear();
        }
    }
}