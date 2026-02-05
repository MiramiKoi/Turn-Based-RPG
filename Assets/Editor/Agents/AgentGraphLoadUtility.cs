using System.Collections.Generic;
using System.IO;
using System.Linq;
using Editor.Agents.Nodes;
using fastJSON;
using Runtime.Agents.Nodes;
using UnityEditor;
using UnityEngine;

namespace Editor.Agents
{
    public class AgentGraphLoadUtility
    {
        private readonly AgentGraphView _graphView;

        public AgentGraphLoadUtility(AgentGraphView graphView)
        {
            _graphView = graphView;
        }

        public void Load()
        {
            
        }
        
        public void LoadEditor()
        {
            _graphView.ClearGraph();
            
            var path = EditorUtility.OpenFilePanel
            ("Load Controllable Behavior", 
                Application.dataPath, 
                "json");

            var json = File.ReadAllText(path);

            var agentBehaviorTree = new AgentDecisionRoot();

            var agentWrapper = DeserializeRecursive(JSON.ToObject<Dictionary<string, object>>(json));
            
            BuildGraphViewRecursive(agentWrapper);
        }
        
        private void BuildGraphViewRecursive(AgentNodeEditorWrapper wrapper)
        {
            var nodeView = _graphView.AddAgentNode(wrapper.Node);
            nodeView.Data = wrapper; 

            foreach (var childWrapper in wrapper.ChildWrappers)
            {
                BuildGraphViewRecursive(childWrapper);

                var childView = _graphView.nodes
                    .OfType<AgentBaseNodeView>()
                    .First(nv => nv.Data == childWrapper);

                wrapper.AddChild(childWrapper); 

                var edge = nodeView.OutputPort.ConnectTo(childView.InputPort);
                _graphView.AddElement(edge);
            }

            wrapper.SortChildrenByPositionX();
        }
        
        public AgentNodeEditorWrapper DeserializeRecursive(Dictionary<string, object> data)
        {
            var node = AgentNode.CreateNodeFromData(data); 
            node.Deserialize(data);
            
            var wrapper = new AgentNodeEditorWrapper(node);
            wrapper.Deserialize(data);

            if (data.TryGetValue("children", out var rawChildren))
            {
                foreach (var childObj in (List<object>)rawChildren)
                {
                    var childDict = (Dictionary<string, object>)childObj;
                    var childWrapper = DeserializeRecursive(childDict);

                    wrapper.AddChild(childWrapper);
                }
                
                wrapper.SortChildrenByPositionX();
            }

            return wrapper;
        }
    }
}