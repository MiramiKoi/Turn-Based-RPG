using System.Collections.Generic;
using System.Linq;
using Editor.Agents.Nodes;
using Runtime.Agents.Nodes;
using Runtime.Extensions;

namespace Editor.Agents.Utilities
{
    public class AgentGraphSerializer
    {
        private const string ChildrenKey = "children";
        
        private const string PositionKey = "position";

        private const string EditorKey = "_editor";
        
        public Dictionary<string, object> Serialize(AgentNodeEditorWrapper wrapper)
        {
            var dict = wrapper.Node.Serialize();

            dict[EditorKey] = new Dictionary<string, object>
            {
                { PositionKey, wrapper.Position.ToList() }
            };

            var childrenList = wrapper.ChildWrappers
                .Select(Serialize)
                .ToList();

            dict[ChildrenKey] = childrenList;

            return dict;
        }

        public AgentNodeEditorWrapper Deserialize(Dictionary<string, object> data)
        {
            var node = AgentNode.CreateNodeFromData(data); 
            node.Deserialize(data);
            
            var wrapper = new AgentNodeEditorWrapper(node);
            wrapper.Deserialize(data);

            if (data.TryGetValue(ChildrenKey, out var rawChildren))
            {
                foreach (var childObj in (List<object>)rawChildren)
                {
                    var childDict = (Dictionary<string, object>)childObj;
                    var childWrapper = Deserialize(childDict);

                    wrapper.AddChild(childWrapper);
                }
                
                wrapper.SortChildrenByPositionX();
            }

            return wrapper;
        }
    }
}