using System.Collections.Generic;
using System.Linq;
using Runtime.Agents.Nodes;
using Runtime.Extensions;
using Runtime.ModelCollections;
using UnityEngine;

namespace Editor.Agents.Nodes
{
    public class AgentNodeEditorWrapper : ISerializable, IDeserializable
    {
        private const string PositionKey = "position";
        
        public Vector2 Position { get; set; }
        
        public AgentNode Node { get; private set;}

        private readonly List<AgentNodeEditorWrapper> _children = new();
        
        public AgentNodeEditorWrapper(AgentNode node)
        {
            Node = node;
        }

        public Dictionary<string, object> Serialize()
        {
            var dictionary = Node.Serialize();
            
            dictionary[PositionKey] = Position;
            
            return dictionary;
        }

        public void Deserialize(Dictionary<string, object> data)
        {
            Node.Deserialize(data);

            Position = data.GetVector2(PositionKey);
        }
        
        public void SortChildrenByPositionX()
        {
            if (_children == null || _children.Count == 0)
                return;

            _children.Sort((a, b) =>
                a.Position.x.CompareTo(b.Position.x));

            Node.Children = _children
                .Select(w => w.Node)
                .ToList();
        }

        public void AddChild(AgentNodeEditorWrapper child)
        {
            if (child == null)
                return;

            if (_children.Contains(child))
                return;

            _children.Add(child);
            Node.Children.Add(child.Node);
        }

        public void RemoveChild(AgentNodeEditorWrapper child)
        {
            if (child == null)
                return;

            if (!_children.Remove(child))
                return;

            Node.Children.Remove(child.Node);
        }
    }
}