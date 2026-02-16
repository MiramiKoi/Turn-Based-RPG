using Runtime.SpawnDirector.Rules;
using UniRx;

namespace Runtime.SpawnDirector
{
    public class SpawnDirectorModel
    {
        public ReactiveDictionary<string, SpawnRuleModel> Rules { get; } = new();

        public void AddRule(SpawnRuleModel rule)
        {
            Rules.Add(rule.Description.Id, rule);
        }

        public void RemoveRule(string id)
        {
            Rules.Remove(id);
        }

        public SpawnRuleModel GetRule(string id)
        {
            return Rules[id];
        }
    }
}