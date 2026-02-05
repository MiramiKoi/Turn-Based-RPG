using Runtime.Agents.Nodes;

namespace Runtime.Descriptions.Agents
{
    public class LogCommand : CommandDescription
    {
        public LogCommand(string name) : base(name)
        {
        }

        protected override string Type => "log";

        public override NodeStatus Execute(IWorldContext context, IControllable controllable)
        {
            return NodeStatus.Success;
        }
    }
}