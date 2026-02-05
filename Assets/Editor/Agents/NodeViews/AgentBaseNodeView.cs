using Runtime.Agents.Nodes;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Editor.Agents.NodeViews
{
    public class AgentBaseNodeView : Node 
    {
        public AgentNode Data { get; }

        public Port InputPort { get; set; }

        public Port OutputPort { get; set; }

        public string Title
        {
            get => base.title;
            set => base.title = value;
        }

        public AgentBaseNodeView(AgentNode data)
        {
            Data = data;
            
            Title = data.Type;
            
            InputPort = InstantiatePort(Orientation.Vertical, Direction.Input, Port.Capacity.Single, typeof(float));
            inputContainer.Add(InputPort);

            OutputPort = InstantiatePort(Orientation.Vertical, Direction.Output, Port.Capacity.Multi, typeof(float));
            outputContainer.Add(OutputPort);
            
            RefreshExpandedState();
            RefreshPorts();
            
            SetPosition(new Rect(data.Position, new Vector2(100, 100)));
        }

        public virtual void SaveData()
        {
            SortPortsByPositionX();
            Data.Position = GetPosition().position;
        }
        
        private void SortPortsByPositionX()
        {
            var sequence = Data as AgentSequence;
            
            sequence?.Children.Sort((a, b) 
                => a.Position.x.CompareTo(b.Position.x));
        }
    }
}