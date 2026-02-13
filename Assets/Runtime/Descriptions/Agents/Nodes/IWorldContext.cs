using Runtime.Landscape.Grid;
using Runtime.TurnBase;
using Runtime.Units.Collection;

namespace Runtime.Descriptions.Agents.Nodes
{
    public interface IWorldContext
    {
        GridModel GridModel { get; }
        UnitModelCollection UnitCollection { get; }
        TurnBaseModel TurnBaseModel { get; }
    }
}