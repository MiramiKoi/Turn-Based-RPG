using System.Collections;
using System.Collections.Generic;

namespace Runtime.Descriptions.Units
{
    public class UnitDescriptionCollection : IEnumerable<UnitDescription>
    {
        private readonly Dictionary<string, UnitDescription> _descriptions;

        public UnitDescriptionCollection(Dictionary<string, object> data)
        {
            _descriptions = new Dictionary<string, UnitDescription>();

            foreach (var description in data)
            {
                _descriptions.Add(description.Key,
                    new UnitDescription(description.Key, (Dictionary<string, object>)description.Value));
            }
        }

        public UnitDescription this[string id] => _descriptions[id];

        public IEnumerator<UnitDescription> GetEnumerator()
        {
            return _descriptions.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}