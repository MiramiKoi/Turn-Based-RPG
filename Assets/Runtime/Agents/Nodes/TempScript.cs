using UnityEngine;

namespace Runtime.Agents.Nodes
{
    public class TempScript : MonoBehaviour
    {
        private void Start()
        {
            var behaviorTree = new BehaviorTree();
            
            behaviorTree.AddChild(new Selector());
            behaviorTree.AddChild(new Selector());
            
            var sequence = new Sequence();
            
            sequence.AddChild(new Sequence());
            sequence.AddChild(new Sequence());
            
            behaviorTree.AddChild(sequence);
            
            var json = JsonUtility.ToJson(behaviorTree);
            
            Debug.Log(json);
        }
    }
}