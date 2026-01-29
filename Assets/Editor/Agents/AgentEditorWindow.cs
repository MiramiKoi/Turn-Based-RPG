using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using fastJSON;
using Runtime.Agents.Nodes;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Object = System.Object;

namespace Editor.Agents
{
    public class AgentEditorWindow : EditorWindow
    {
        private const string Title = "Agent Behavior Tree Editor";

        private AgentGraphView _graphView;

        private void OnEnable()
        {
            AddStyles();

            _graphView = new AgentGraphView();

            _graphView.StretchToParentSize();

            rootVisualElement.Add(_graphView);

            rootVisualElement.Add(CreateToolbar());

            _graphView.graphViewChanged += OnGraphElementChanged;
        }

        private void OnDisable()
        {
            _graphView.graphViewChanged -= OnGraphElementChanged;
        }

        [MenuItem("Window/Agent Behavior Tree Editor")]
        public static void ShowEditor()
        {
            var window = GetWindow<AgentEditorWindow>();

            window.titleContent = new GUIContent(Title);
        }

        private void AddStyles()
        {
            rootVisualElement.styleSheets.Add(Resources.Load<StyleSheet>("DialogueEditorStyles"));
        }

        private Toolbar CreateToolbar()
        {
            var toolbar = new Toolbar();

            toolbar.Add(CreateToolbarButton("Selector", _graphView.AddAgentNode<AgentSelector>));
            toolbar.Add(CreateToolbarButton("Sequence", _graphView.AddAgentNode<AgentSequence>));
            toolbar.Add(CreateToolbarButton("Leaf", _graphView.AddAgentNode<AgentLeaf>));
            toolbar.Add(CreateToolbarButton("Root", _graphView.AddAgentNode<AgentBehaviorTree>));

            toolbar.Add(CreateToolbarButton("Save", Save));

            toolbar.Add(CreateToolbarButton("Load", Load));

            toolbar.Add(CreateToolbarButton("Clear", () => _graphView.ClearGraph()));

            return toolbar;
        }

        private ToolbarButton CreateToolbarButton(string text, Action onClick = null)
        {
            var button = new ToolbarButton
            {
                text = text
            };

            if (onClick != null) button.clicked += onClick;

            return button;
        }

        private void Save()
        {
            var path = EditorUtility.SaveFilePanel(
                "Save Agent Behavior",
                Application.dataPath,
                "behavior-tree.json",
                "json");

            if (string.IsNullOrEmpty(path))
            {
                return;
            }

            var nodes = _graphView.nodes.OfType<AgentBaseNodeView>().ToList(); 
            
            nodes.ToList().ForEach(nv => nv.SaveData());
            
            var rootNodeView = nodes.FirstOrDefault(nv => nv.Data is AgentBehaviorTree);

            if (rootNodeView == null)
            {
                return;
            }

            var json = JSON.ToNiceJSON(rootNodeView.Data.Serialize());
            
            File.WriteAllText(path, json);
        }

        private void Load()
        {
            _graphView.ClearGraph();
            
            var path = EditorUtility.OpenFilePanel
                ("Load Agent Behavior", 
                Application.dataPath, 
                "json");

            Debug.Log(path);
            
            var json = File.ReadAllText(path);

            var agentBehaviorTree = new AgentBehaviorTree();
            
            agentBehaviorTree.Deserialize(JSON.ToObject<Dictionary<string, object>>(json));
            
            BuildGraphRecursive(agentBehaviorTree);
        }

        private GraphViewChange OnGraphElementChanged(GraphViewChange graphViewChange)
        {
            if (graphViewChange.edgesToCreate != null)
            {
                foreach (var edge in graphViewChange.edgesToCreate)
                {
                    var outputNodeView = edge.output.node as AgentBaseNodeView;
                    var inputNodeView = edge.input.node as AgentBaseNodeView;

                    if (!outputNodeView.Data.Children.Contains(inputNodeView.Data))
                    {
                        outputNodeView.Data.AddChild(inputNodeView.Data);
                        Debug.Log($"Added child: {inputNodeView.Data.Type} to {outputNodeView.Data.Type}");
                    }
                }
            }

            if (graphViewChange.elementsToRemove != null)
            {
                foreach (var edge in graphViewChange.elementsToRemove.OfType<Edge>())
                {
                    var outputNodeView = edge.output.node as AgentBaseNodeView;
                    var inputNodeView = edge.input.node as AgentBaseNodeView;

                    if (outputNodeView.Data.Children.Contains(inputNodeView.Data))
                    {
                        outputNodeView.Data.Children.Remove(inputNodeView.Data);
                        Debug.Log($"Removed child: {inputNodeView.Data.Type} from {outputNodeView.Data.Type}");
                    }
                }
            }
            
            return graphViewChange;
        }

        private void BuildGraphRecursive(AgentNode node)
        {
            var nodeView = _graphView.AddAgentNode(node);

            foreach (var child in node.Children)
            {
                BuildGraphRecursive(child);
                
                var childView = _graphView.nodes.ToList()
                    .OfType<AgentBaseNodeView>()
                    .First(nv => nv.Data ==  child);
                
                var edge = nodeView.OutputPort.ConnectTo(childView.InputPort);
                _graphView.AddElement(edge);
            }
        }
    }
}