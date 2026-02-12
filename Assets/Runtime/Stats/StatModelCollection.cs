using System.Collections;
using System.Collections.Generic;
using Runtime.Descriptions.Stats;
using Runtime.ModelCollections;

namespace Runtime.Stats
{
    public class StatModelCollection : IEnumerable<StatModel>, ISerializable
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

        public StatModel Get(string id)
        {
            return Stats[id];
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

        public Dictionary<string, object> Serialize()
        {
            return new Dictionary<string, object> { };
        }
    }
}