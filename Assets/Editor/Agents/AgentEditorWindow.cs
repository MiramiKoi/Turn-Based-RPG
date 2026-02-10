using System;
using System.Linq;
using Editor.Agents.Nodes;
using Editor.Agents.Utilities;
using Runtime.Descriptions.Agents.Nodes;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Edge = UnityEditor.Experimental.GraphView.Edge;

namespace Editor.Agents
{
    public class AgentEditorWindow : EditorWindow
    {
        private const string Title = "Controllable Behavior Tree Editor";

        private AgentGraphView _graphView;

        private AgentGraphSaveUtility _saveUtility;

        private AgentGraphLoadUtility _loadUtility;

        private AgentGraphSerializer _serializer;

        private void OnEnable()
        {
            AddStyles();

            _graphView = new AgentGraphView();

            _serializer = new AgentGraphSerializer();

            _saveUtility = new AgentGraphSaveUtility(_graphView, _serializer);

            _loadUtility = new AgentGraphLoadUtility(_graphView, _serializer);

            _graphView.StretchToParentSize();

            rootVisualElement.Add(_graphView);

            rootVisualElement.Add(CreateToolbar());

            _graphView.graphViewChanged += OnGraphElementChanged;
        }

        private void OnDisable()
        {
            _graphView.graphViewChanged -= OnGraphElementChanged;
        }

        [MenuItem("Window/Controllable Behavior Tree Editor")]
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
            toolbar.Add(CreateToolbarButton("Inverse", _graphView.AddAgentNode<AgentInverse>));
            toolbar.Add(CreateToolbarButton("Root", _graphView.AddAgentNode<AgentDecisionDescription>));

            toolbar.Add(CreateToolbarButton("Save", _saveUtility.Save));
            toolbar.Add(CreateToolbarButton("Bake", _saveUtility.Bake));
            toolbar.Add(CreateToolbarButton("Load", _loadUtility.Load));
            toolbar.Add(CreateToolbarButton("Clear", _graphView.ClearGraph));

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

        private GraphViewChange OnGraphElementChanged(GraphViewChange graphViewChange)
        {
            if (graphViewChange.edgesToCreate != null)
                foreach (var edge in graphViewChange.edgesToCreate)
                {
                    if (edge.output.node is not AgentBaseNodeView outputNodeView ||
                        edge.input.node is not AgentBaseNodeView inputNodeView)
                        continue;

                    outputNodeView.Data.AddChild(inputNodeView.Data);
                }

            if (graphViewChange.elementsToRemove != null)
                foreach (var edge in graphViewChange.elementsToRemove.OfType<Edge>())
                {
                    if (edge.output.node is not AgentBaseNodeView outputNodeView ||
                        edge.input.node is not AgentBaseNodeView inputNodeView)
                        continue;

                    outputNodeView.Data.RemoveChild(inputNodeView.Data);
                }

            return graphViewChange;
        }
    }
}