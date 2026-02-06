using Runtime.Landscape.Grid;
using Runtime.Units;

namespace Runtime.Descriptions.Agents.Nodes
{
    public interface IWorldContext
    {
        GridModel GridModel { get; }
        UnitModelCollection UnitCollection { get; }
    }
}