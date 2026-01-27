using System;
using System.Collections.Generic;
using Runtime.Extensions;
using Runtime.ModelCollections;

namespace Runtime.Agents.Nodes
{
    public abstract class Node : ISerializable, IDeserializable
    {
        private const string TypeKey = "type";
        
        private const string ChildrenKey = "children";
        
        protected abstract string Type { get; }
        
        protected List<Node> Children { get; set; } = new List<Node>();

        protected int CurrentChildIndex { get; set; } = 0;
        
        public void AddChild(Node child)
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
            Node node = CreateNodeFromData(data);

            if (data.TryGetValue(ChildrenKey, out var childrenObj) && childrenObj is List<object> childrenList)
            {
                foreach (var childObj in childrenList)
                {
                    if (childObj is Dictionary<string, object> childDict)
                    {
                        Node childNode = CreateNodeFromData(childDict);
                        childNode.Deserialize(childDict);
                        node.AddChild(childNode);
                    }
                }
            }

            Children = node.Children;
        }

        private static Node CreateNodeFromData(Dictionary<string, object> data)
        {
            var typeString = data.GetString(TypeKey).ToLowerInvariant();

            return typeString switch
            {
                "selector" => new Selector(),
                "sequence" => new Sequence(),
                "leaf" => new Leaf(),
                _ => throw new Exception()
            };
        }
    }
}