using System.Collections.Generic;
using System.IO;
using System.Linq;
using Editor.Agents.Nodes;
using fastJSON;
using UnityEditor;
using UnityEngine;

namespace Editor.Agents.Utilities
{
    public class AgentGraphLoadUtility
    {
        private readonly AgentGraphView _graphView;
        private readonly AgentGraphSerializer _serializer;

        public AgentGraphLoadUtility(AgentGraphView graphView, AgentGraphSerializer serializer)
        {
            _graphView = graphView;
            _serializer = serializer;
        }

        public void Load()
        {
            _graphView.ClearGraph();

            var path = EditorUtility.OpenFilePanel
            ("Load Controllable Behavior",
                Application.dataPath,
                "json");

            var json = File.ReadAllText(path);

            var agentWrapper = _serializer.Deserialize(JSON.ToObject<Dictionary<string, object>>(json));

            BuildGraphViewRecursive(agentWrapper);
        }

        private void BuildGraphViewRecursive(AgentNodeEditorWrapper wrapper)
        {
            var nodeView = _graphView.AddAgentNode(wrapper);

            foreach (var childWrapper in wrapper.ChildWrappers)
            {
                BuildGraphViewRecursive(childWrapper);

                var childView = _graphView.nodes
                    .OfType<AgentBaseNodeView>()
                    .First(nv => nv.Data == childWrapper);

                var edge = nodeView.OutputPort.ConnectTo(childView.InputPort);
                _graphView.AddElement(edge);
            }
        }
    }
}