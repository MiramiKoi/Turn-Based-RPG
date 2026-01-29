using System.Collections.Generic;

namespace Runtime.ModelCollections
{
    public interface IDeserializable
    {
        void Deserialize(Dictionary<string, object> data);
    }
}