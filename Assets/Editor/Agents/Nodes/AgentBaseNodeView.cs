using Runtime.Agents.Nodes;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Editor.Agents.Nodes
{
    public class AgentBaseNodeView : Node 
    {
        public AgentNodeEditorWrapper Data { get; }

        public Port InputPort { get; set; }

        public Port OutputPort { get; set; }

        public string Title
        {
            get => base.title;
            set => base.title = value;
        }

        public AgentBaseNodeView(AgentNodeEditorWrapper data)
        {
            Data = data;

            SetupNode();
        }

        public AgentBaseNodeView(AgentNode data)
        {
            Data = new AgentNodeEditorWrapper(data);
            
            SetupNode();
        }

        private void SetupNode()
        {
            Title = Data.Node.Type;
            
            InputPort = InstantiatePort(Orientation.Vertical, Direction.Input, Port.Capacity.Single, typeof(float));
            inputContainer.Add(InputPort);

            OutputPort = InstantiatePort(Orientation.Vertical, Direction.Output, Port.Capacity.Multi, typeof(float));
            outputContainer.Add(OutputPort);
            
            RefreshExpandedState();
            RefreshPorts();
            
            SetPosition(new Rect(Data.Position, new Vector2(100, 100)));
        }

        public virtual void SaveData()
        {
            Data.SortChildrenByPositionX();
            Data.Position = GetPosition().position;
        }
    }
}