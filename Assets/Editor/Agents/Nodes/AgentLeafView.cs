using Runtime.Agents.Nodes;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.Agents.Nodes
{
    public class AgentLeafView : AgentBaseNodeView
    {
        private AgentLeaf LeafData => Data.Node as AgentLeaf;
        
        private TextField _commandTextField;

        public AgentLeafView(AgentNodeEditorWrapper wrapper) : base(wrapper)
        {
            
        }
        
        public AgentLeafView(AgentLeaf data) : base(data)
        {

        }

        public override void SaveData()
        {
            base.SaveData();
            
            LeafData.Command = _commandTextField.text; 
        }

        protected override void Setup()
        {
            base.Setup();
            
            outputContainer.Clear();
            
            Debug.Log($"{Data.Node is AgentLeaf}: {LeafData.Command}");
                
            _commandTextField = new TextField()
            {
                name = "command",
                multiline = false,
                value = LeafData.Command
            };
            
            titleContainer.Add(_commandTextField);
        }
    }
}