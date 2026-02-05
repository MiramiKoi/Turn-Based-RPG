using System.Collections;
using System.Collections.Generic;
using Runtime.Descriptions.Stats;

namespace Runtime.Stats
{
    public class StatModelCollection : IEnumerable<StatModel>
    {
        public Dictionary<string, StatModel> Stats { get; }
        
        public StatModelCollection(StatDescriptionCollection descriptionCollection)
        {
            Stats = new Dictionary<string, StatModel>();

            foreach (var description in descriptionCollection)
            {
                Stats.Add(description.Id, new StatModel(description));
            }
        }

        public IEnumerator<StatModel> GetEnumerator()
        {
            return Stats.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public StatModel this[string id] => Stats[id];
    }
}