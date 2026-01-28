using Runtime.Agents.Nodes;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Editor.Agents
{
    public class AgentNodeView : Node
    {
        public Port InputPort { get; set; }

        public Port OutputPort { get; set; }
        
        public AgentNode Data { get; set; }

        public AgentNodeView(AgentNode data)
        {
            title = data.Type;
            Data = data;
            
            InputPort = InstantiatePort(Orientation.Vertical, Direction.Input, Port.Capacity.Single, typeof(float));
            outputContainer.Add(InputPort);

            OutputPort = InstantiatePort(Orientation.Vertical, Direction.Output, Port.Capacity.Multi, typeof(float));
            outputContainer.Add(OutputPort);
            
            RefreshExpandedState();
            RefreshPorts();
            SetPosition(new Rect(data.Position, new Vector2(100, 100)));
        }

        public void SavePosition()
        {
            Debug.Log($"Position: {GetPosition().position}");
            
            Data.Position = GetPosition().position;
        }
    }
}