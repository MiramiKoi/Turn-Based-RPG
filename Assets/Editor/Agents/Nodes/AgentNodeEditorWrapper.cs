using System.Collections.Generic;
using System.Linq;
using Runtime.Descriptions.Agents.Nodes;
using Runtime.Extensions;
using Runtime.ModelCollections;
using UnityEngine;

namespace Editor.Agents.Nodes
{
    public class AgentNodeEditorWrapper : IDeserializable
    {
        private const string PositionKey = "position";
        private const string EditorKey = "_editor";

        public Vector2 Position { get; set; }

        public AgentNode Node { get; }

        public readonly List<AgentNodeEditorWrapper> ChildWrappers = new();

        public AgentNodeEditorWrapper(AgentNode node)
        {
            Node = node;
        }

        public void Deserialize(Dictionary<string, object> data)
        {
            if (data.TryGetValue(EditorKey, out var founded))
            {
                var editor = founded as Dictionary<string, object>;

                Position = editor.GetVector2(PositionKey);
            }
        }

        public void SortChildrenByPositionX()
        {
            ChildWrappers.Sort((a, b) =>
                a.Position.x.CompareTo(b.Position.x));

            Node.Children = ChildWrappers
                .Select(w => w.Node)
                .ToList();
        }


        public void AddChild(AgentNodeEditorWrapper child)
        {
            ChildWrappers.Add(child);
            Node.Children.Add(child.Node);
        }

        public void RemoveChild(AgentNodeEditorWrapper child)
        {
            if (child == null)
                return;

            if (!ChildWrappers.Remove(child))
                return;

            Node.Children.Remove(child.Node);
        }
    }
}