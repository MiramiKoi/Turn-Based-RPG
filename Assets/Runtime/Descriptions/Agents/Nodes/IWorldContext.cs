using Runtime.Landscape.Grid;
using Runtime.Units.Collection;

namespace Runtime.Descriptions.Agents.Nodes
{
    public interface IWorldContext
    {
        GridModel GridModel { get; }
        UnitModelCollection UnitCollection { get; }
    }
}