using System.Collections.Generic;

namespace Runtime.ModelCollections
{
    public interface ISerializable
    {
        Dictionary<string, object> Serialize();
    }
}