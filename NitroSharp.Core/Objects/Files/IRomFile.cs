using NitroSharp.Core.Data;
using NitroSharp.Core.Data.Archive;

namespace NitroSharp.Core.Objects.Files;

public interface IRomFile
{
    string Path { get; }
    string Name { get; }
    uint Offset { get; }
    uint Size { get; }
    ushort Id { get; }

    DataArchive Open();

    T Load<T>() where T : INitroDeserializable<T>;
}
