using Runtime.Agents.Nodes;

namespace Editor.Agents
{
    public class AgentSequenceView : AgentBaseNodeView
    {
        public AgentSequenceView(AgentSequence data) : base(data)
        {
        }

        public void SortPortsByPositionX()
        {
            var sequence = Data as AgentSequence;
            
            sequence?.Children.Sort((a, b) 
                => a.Position.x.CompareTo(b.Position.x));
        }

        public override void SaveData()
        {
            base.SaveData();
            
            SortPortsByPositionX();
        }
    }
}