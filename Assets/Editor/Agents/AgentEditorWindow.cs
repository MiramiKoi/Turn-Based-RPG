using System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

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

            toolbar.Add(CreateToolbarButton("Create node", () => _graphView.AddDialogueNode()));

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
            
        }

        private void Load()
        {
            
        }
    }
}