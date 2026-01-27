using System;
using System.Collections.Generic;
using Runtime.Extensions;
using Runtime.ModelCollections;

namespace Runtime.Agents.Nodes
{
    public abstract class AgentNode : ISerializable, IDeserializable
    {
        private const string TypeKey = "type";
        
        private const string ChildrenKey = "children";
        
        protected abstract string Type { get; }
        
        protected List<AgentNode> Children { get; set; } = new List<AgentNode>();

        protected int CurrentChildIndex { get; set; } = 0;
        
        public void AddChild(AgentNode child)
        {
            Children.Add(child);
        }

        public virtual NodeStatus Process(IWorldContext context, IUnit unit)
        {
            return Children[CurrentChildIndex].Process(context, unit);
        }

        public virtual void Reset()
        {
            CurrentChildIndex = 0;
            
            foreach (var child in Children)
            {
                child.Reset();
            }
        }

        public virtual Dictionary<string, object> Serialize()
        {
            var children = new List<Dictionary<string, object>>();

            foreach (var child in Children)
            {
                children.Add(child.Serialize());
            }
            
            return new Dictionary<string, object>()
            {
                {TypeKey, Type},
                {ChildrenKey, children}
            };
        }

        public virtual void Deserialize(Dictionary<string, object> data)
        {
            AgentNode agentNode = CreateNodeFromData(data);

            if (data.TryGetValue(ChildrenKey, out var childrenObj) && childrenObj is List<object> childrenList)
            {
                foreach (var childObj in childrenList)
                {
                    if (childObj is Dictionary<string, object> childDict)
                    {
                        AgentNode childAgentNode = CreateNodeFromData(childDict);
                        childAgentNode.Deserialize(childDict);
                        agentNode.AddChild(childAgentNode);
                    }
                }
            }

            Children = agentNode.Children;
        }

        private static AgentNode CreateNodeFromData(Dictionary<string, object> data)
        {
            var typeString = data.GetString(TypeKey).ToLowerInvariant();

            return typeString switch
            {
                "selector" => new AgentSelector(),
                "sequence" => new AgentSequence(),
                "leaf" => new AgentLeaf(),
                _ => throw new Exception()
            };
        }
    }
}