using fastJSON;
using UnityEngine;

namespace Runtime.Agents.Nodes
{
    public class TempScript : MonoBehaviour
    {
        private void Start()
        {
            var behaviorTree = new AgentBehaviorTree();
            
            behaviorTree.AddChild(new AgentSelector());
            behaviorTree.AddChild(new AgentSelector());
            
            var sequence = new AgentSequence();
            
            sequence.AddChild(new AgentSequence());
            sequence.AddChild(new AgentSequence());
            
            behaviorTree.AddChild(sequence);
            
            var json = JSON.ToNiceJSON(behaviorTree.Serialize());
            
            Debug.Log(json);
        }
    }
}