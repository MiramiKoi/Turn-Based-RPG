using Runtime.Landscape.Grid;

namespace Runtime.Descriptions.Agents.Nodes
{
    public interface IWorldContext
    {
        GridModel GridModel { get; }
    }
}