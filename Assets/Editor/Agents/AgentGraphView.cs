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

        public void AddDialogueNode()
        {
            
        }

        public void ClearGraph()
        {
        }

        private void SetupManipulators()
        {
            this.AddManipulator(new ContentDragger());

            this.AddManipulator(new SelectionDragger());

            this.AddManipulator(new RectangleSelector());
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