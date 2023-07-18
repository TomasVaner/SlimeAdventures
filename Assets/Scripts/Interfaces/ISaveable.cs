using System;
using Utility;

namespace Interfaces
{
    public interface ISaveable
    {
        public SerializableGuid ID { get; }
        
        Tuple<SerializableGuid, object> Save();
        void Load(Tuple<SerializableGuid, object> state);

    }
}