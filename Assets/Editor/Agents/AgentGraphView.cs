using System;
using System.Collections.Generic;
using Editor.Agents.Nodes;
using Runtime.Descriptions.Agents.Nodes;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.Agents
{
    public class AgentGraphView : GraphView
    {
        public AgentGraphView()
        {
            AddStyles();
            
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
            
            SetupManipulators();

            SetupGridBackground();
            
            ClearGraph();
        }

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            return ports.ToList();
        }

        public AgentBaseNodeView AddAgentNode(AgentNodeEditorWrapper wrapper)
        {
            AgentBaseNodeView agentNodeView = wrapper.Node switch
            {
                AgentSequence agentSequence => new AgentSequenceView(wrapper),
                AgentSelector agentSelector => new AgentSelectorView(wrapper),
                AgentDecisionDescription agentBehaviorTree => new AgentDecisionNodeView(wrapper),
                AgentLeaf agentLeaf => new AgentLeafView(wrapper),
                _ => throw new SystemException()
            };
            
            AddElement(agentNodeView);
            
            return agentNodeView;
        }
        
        public AgentBaseNodeView AddAgentNode(AgentNode data)
        {
            AgentBaseNodeView agentNodeView = data switch
            {
                AgentSequence agentSequence => new AgentSequenceView(agentSequence),
                AgentSelector agentSelector => new AgentSelectorView(agentSelector),
                AgentDecisionDescription agentBehaviorTree => new AgentDecisionNodeView(agentBehaviorTree),
                AgentLeaf agentLeaf => new AgentLeafView(agentLeaf),
                _ => throw new SystemException()
            };
            
            AddElement(agentNodeView);

            return agentNodeView;
        }
        
        public void AddAgentNode<TData>() where TData : AgentNode, new()
        {
            var data = new TData();

            AddAgentNode(data);
        }
        
        public void ClearGraph()
        {
            graphElements.ForEach(RemoveElement);
        }
        
        private void SetupManipulators()
        {
            this.AddManipulator(new ContentDragger());

            this.AddManipulator(new SelectionDragger());

            this.AddManipulator(new RectangleSelector());
            
            this.AddManipulator(new FreehandSelector());

        }

        private void SetupGridBackground()
        {
            var gridBackground = new GridBackground();

            gridBackground.style.backgroundColor = Color.black;
            
            gridBackground.StretchToParentSize();

            Insert(0, gridBackground);
        }

        private void AddStyles()
        {
            styleSheets.Add(Resources.Load<StyleSheet>("GraphViewStyles"));
        }
    }
}