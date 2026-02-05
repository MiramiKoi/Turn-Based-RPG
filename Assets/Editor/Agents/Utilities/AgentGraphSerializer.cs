using System.Collections.Generic;
using System.Linq;
using Editor.Agents.Nodes;
using Runtime.Agents.Nodes;
using Runtime.Extensions;

namespace Editor.Agents
{
    public class AgentGraphSerializer
    {
        public Dictionary<string, object> Serialize(AgentNodeEditorWrapper wrapper)
        {
            var dict = wrapper.Node.Serialize();

            dict["_editor"] = new Dictionary<string, object>
            {
                { "position", wrapper.Position.ToList() }
            };

            var childrenList = wrapper.ChildWrappers
                .Select(Serialize)
                .ToList();

            dict["children"] = childrenList;

            return dict;
        }

        public AgentNodeEditorWrapper Deserialize(Dictionary<string, object> data)
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
                    var childWrapper = Deserialize(childDict);

                    wrapper.AddChild(childWrapper);
                }
                
                wrapper.SortChildrenByPositionX();
            }

            return wrapper;
        }
    }
}