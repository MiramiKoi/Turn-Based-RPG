using Runtime.SpawnDirector.Rules;
using UniRx;

namespace Runtime.SpawnDirector
{
    public class SpawnDirectorModel
    {
        public ReactiveCollection<ISpawnRule> Rules { get; } = new();
        
        public void AddRule(ISpawnRule rule)
        {
            Rules.Add(rule);
        }

        public void RemoveRule(ISpawnRule rule)
        {
            Rules.Remove(rule);
        }
    }
}