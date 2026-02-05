using Runtime.Stats;

namespace Runtime.Common
{
    public interface IUnit
    {
        string Id { get; }
        StatModel Health { get; }
    }
}