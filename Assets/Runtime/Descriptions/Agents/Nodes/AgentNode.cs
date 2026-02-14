using System;
using System.Collections.Generic;
using Runtime.Extensions;
using Runtime.ModelCollections;

namespace Runtime.Descriptions.Agents.Nodes
{
    public abstract class AgentNode : ISerializable, IDeserializable
    {
        private const string TypeKey = "type";

        private const string ChildrenKey = "children";

        private const string NameKey = "name";

        public event Action OnAddChild;

        public abstract string Type { get; }

        public string Name { get; set; } = string.Empty;

        public List<AgentNode> Children { get; set; } = new();

        public void AddChild(AgentNode child)
        {
            Children.Add(child);

            OnAddChild?.Invoke();
        }

        public abstract NodeStatus Process(IWorldContext context, IControllable controllable);

        public virtual Dictionary<string, object> Serialize()
        {
            var children = new List<Dictionary<string, object>>();

            foreach (var child in Children)
            {
                children.Add(child.Serialize());
            }

            return new Dictionary<string, object>
            {
                { TypeKey, Type },
                { ChildrenKey, children },
                { NameKey, Name }
            };
        }

        public virtual void Deserialize(Dictionary<string, object> data)
        {
            var agentNode = CreateNodeFromData(data);

            if (data.TryGetValue(ChildrenKey, out var childrenObj) && childrenObj is List<object> childrenList)
            {
                foreach (var childObj in childrenList)
                {
                    if (childObj is Dictionary<string, object> childDict)
                    {
                        var childAgentNode = CreateNodeFromData(childDict);
                        childAgentNode.Deserialize(childDict);
                        agentNode.AddChild(childAgentNode);
                    }
                }
            }

            if (data.ContainsKey(NameKey))
            {
                Name = data.GetString(NameKey);
            }

            Children = agentNode.Children;
        }

        public static AgentNode CreateNodeFromData(Dictionary<string, object> data)
        {
            var typeString = data.GetString(TypeKey).ToLowerInvariant();

            AgentNode node = typeString switch
            {
                "selector" => new AgentSelector(),
                "sequence" => new AgentSequence(),
                "leaf" => new AgentLeaf(),
                "root" => new AgentDecisionDescription(),
                "inverse" => new AgentInverse(),
                "link" => new AgentDecisionLink(),
                _ => throw new Exception()
            };

            return node;
        }
    }
}