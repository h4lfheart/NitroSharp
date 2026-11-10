using NitroSharp.Core.Data.Archive;

namespace NitroSharp.Core.Data;

public interface INitroDeserializable<TSelf> where TSelf : INitroDeserializable<TSelf>
{
    static abstract TSelf Deserialize(DataArchive Ar);
}
