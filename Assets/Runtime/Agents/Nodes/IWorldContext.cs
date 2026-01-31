using Runtime.Landscape.Grid;

namespace Runtime.Agents.Nodes
{
    public interface IWorldContext
    {
        GridModel GridModel { get; }
    }
}