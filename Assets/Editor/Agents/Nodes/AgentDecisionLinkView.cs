using Runtime.Descriptions.Agents.Nodes;
using UnityEngine.UIElements;

namespace Editor.Agents.Nodes
{
    public class AgentDecisionLinkView : AgentBaseNodeView
    {
        protected AgentDecisionLink AgentDecisionLink => Data.Node as AgentDecisionLink;
        
        private TextField _linkDescriptionTextField;
        
        public AgentDecisionLinkView(AgentNodeEditorWrapper data) : base(data)
        {
        }

        public AgentDecisionLinkView(AgentNode data) : base(data)
        {
        }


        protected override void Setup()
        {
            base.Setup();
            
            outputContainer.Clear();

            _linkDescriptionTextField = new TextField()
            {
                label = "Description Id",
                value = AgentDecisionLink.DescriptionId                                
            };

            _linkDescriptionTextField.RegisterValueChangedCallback(OnChangeLink);
            
            outputContainer.Add(_linkDescriptionTextField);
        }

        private void OnChangeLink(ChangeEvent<string> evt)
        {
            AgentDecisionLink.DescriptionId = evt.newValue;
        }
    }
}