using System.Collections.Generic;
using System.IO;
using System.Linq;
using Editor.Agents.Nodes;
using fastJSON;
using Runtime.Agents.Nodes;
using Runtime.ModelCollections;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Editor.Agents
{
    public class AgentGraphSaveUtility
    {
        private List<AgentBaseNodeView> Nodes => _graphView.nodes.OfType<AgentBaseNodeView>().ToList();
        
        private readonly GraphView _graphView;
        
        public AgentGraphSaveUtility(GraphView graphView)
        {
            _graphView = graphView;
        }
        
        public void Bake()
        {
            Save(GetRoot().Data, GetPath());
        }
        
        public void Save()
        {
            Save(GetRoot().Data.Node, GetPath());
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

        private void Save(ISerializable serializable, string path)
        {
            Nodes.ForEach(nv => nv.SaveData());
            
            if (string.IsNullOrEmpty(path))
            {
                Debug.LogWarning("[Agent Editor]: Path is empty");
                
                return;
            }
            
            var json = JSON.ToNiceJSON(serializable.Serialize());

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
    }
}