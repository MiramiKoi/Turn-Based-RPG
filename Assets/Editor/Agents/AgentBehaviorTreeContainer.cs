using System.Collections.Generic;
using Runtime.Agents.Nodes;
using UnityEngine;

namespace Editor.Agents
{
    public class AgentBehaviorTreeContainer
    {
        public string Guid;

        public Vector2 Position;

        public List<AgentBehaviorTreeContainer> Children = new List<AgentBehaviorTreeContainer>(); 
        
        public AgentBehaviorTree Data;
    }
}