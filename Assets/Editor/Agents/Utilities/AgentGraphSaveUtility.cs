using System.Collections.Generic;
using System.IO;
using System.Linq;
using Editor.Agents.Nodes;
using fastJSON;
using Runtime.Agents.Nodes;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Editor.Agents.Utilities
{
    public class AgentGraphSaveUtility
    {
        private List<AgentBaseNodeView> Nodes => _graphView.nodes.OfType<AgentBaseNodeView>().ToList();

        private readonly GraphView _graphView;

        private readonly AgentGraphSerializer _serializer;
        
        public AgentGraphSaveUtility(GraphView graphView, AgentGraphSerializer serializer)
        {
            _graphView = graphView;
            _serializer = serializer;
        }

        public void Bake()
        {
            Save(GetRoot().Data.Node, GetPath());
        }

        public void Save()
        {
            Nodes.ForEach(nv => nv.SaveData());
            
            var dict = _serializer.Serialize(GetRoot().Data);
            var json = JSON.ToNiceJSON(dict);
            File.WriteAllText(GetPath(), json);
            
            Save(GetRoot().Data, GetPath());
        }

        private void Save(AgentNode node, string path)
        {
            Save(node.Serialize(), path);
        }

        private void Save(AgentNodeEditorWrapper wrapper, string path)
        {
            Save(_serializer.Serialize(wrapper), path);     
        }

        private void Save(Dictionary<string, object> dictionary, string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                Debug.LogWarning("[Agent Editor]: Path is empty");

                return;
            }

            var json = JSON.ToNiceJSON(dictionary);

            File.WriteAllText(path, json);
        }

        private string GetPath()
        {
            var path = EditorUtility.SaveFilePanel(
                "Save Controllable Behavior",
                Application.dataPath,
                "behavior-tree.json",
                "json");

            return path;
        }

        private AgentBaseNodeView GetRoot()
        {
            var rootNodeView = Nodes.FirstOrDefault(nv => nv.Data.Node is AgentDecisionRoot);

            if (rootNodeView == null)
            {
                throw new KeyNotFoundException("Root node not found");
            }

            return rootNodeView;
        }
    }
}