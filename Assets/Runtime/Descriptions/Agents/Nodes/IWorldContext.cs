using Runtime.Landscape.Grid;
using Runtime.Units.Collection;
using Runtime.TurnBase;

namespace Runtime.Descriptions.Agents.Nodes
{
    public interface IWorldContext
    {
        GridModel GridModel { get; }
        UnitModelCollection UnitCollection { get; }
        TurnBaseModel TurnBaseModel { get; }
    }
}