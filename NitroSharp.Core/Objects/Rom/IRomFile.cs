using NitroSharp.Core.Data.Archive;

namespace NitroSharp.Core.Objects.Rom;

public interface IRomFile
{
    string Path { get; }
    string Name { get; }
    uint Offset { get; }
    uint Size { get; }
    ushort Id { get; }

    byte[] Read();
    DataArchive CreateArchive();
}
