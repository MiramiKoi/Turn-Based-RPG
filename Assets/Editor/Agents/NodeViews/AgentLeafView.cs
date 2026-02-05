using Runtime.Agents.Nodes;
using UnityEngine.UIElements;

namespace Editor.Agents.NodeViews
{
    public class AgentLeafView : AgentBaseNodeView
    {
        private AgentLeaf LeafData => Data as AgentLeaf;
        private readonly TextField _commandTextField;

        public AgentLeafView(AgentLeaf data) : base(data)
        {
            outputContainer.Clear();
                
            _commandTextField = new TextField()
            {
                name = "command",
                multiline = false,
                value = LeafData.Command
            };
            
            titleContainer.Add(_commandTextField);
        }

        public override void SaveData()
        {
            base.SaveData();
            
            LeafData.Command = _commandTextField.text; 
        }
    }
}