using Runtime.Descriptions.Agents.Nodes;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.Agents.Nodes
{
    public class AgentBaseNodeView : Node
    {
        public AgentNodeEditorWrapper Data { get; set; }

        public Port InputPort { get; set; }

        public Port OutputPort { get; set; }

        public string Title
        {
            get => base.title;
            set => base.title = value;
        }

        protected AgentBaseNodeView(AgentNodeEditorWrapper data)
        {
            Data = data;

            Setup();
        }

        protected AgentBaseNodeView(AgentNode data)
        {
            Data = new AgentNodeEditorWrapper(data);

            Setup();
        }

        public virtual void SaveData()
        {
            Data.Position = GetPosition().position;
        }

        protected virtual void Setup()
        {
            Title = Data.Node.Type;

            var nameField = new TextField
            {
                value = Data.Node.Name
            };

            nameField.RegisterValueChangedCallback(evt => { Data.Node.Name = evt.newValue; });

            titleContainer.Add(nameField);


            InputPort = InstantiatePort(Orientation.Vertical, Direction.Input, Port.Capacity.Single, typeof(float));
            inputContainer.Add(InputPort);

            OutputPort = InstantiatePort(Orientation.Vertical, Direction.Output, Port.Capacity.Multi, typeof(float));
            outputContainer.Add(OutputPort);

            RefreshExpandedState();
            RefreshPorts();

            SetPosition(new Rect(Data.Position, new Vector2(100, 100)));
        }
    }
}