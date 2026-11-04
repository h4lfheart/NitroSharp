using NitroSharp.Core.Data.Archive;
using NitroSharp.Core.Objects.Rom;
using NitroSharp.Core.Providers;

namespace NitroSharp.Core.Objects.Rom;

public sealed class RomFile : IRomFile
{
    public NitroFileProvider Provider { get; private set; }

    public string Path { get; }
    public string Name { get; }
    public uint Offset { get; }
    public uint Size { get; }
    public ushort Id { get; }

    public RomFile(NitroFileProvider provider, string path, string name, uint offset, uint size, ushort id)
    {
        Provider = provider;
        Path = path;
        Name = name;
        Offset = offset;
        Size = size;
        Id = id;
    }

    public RomFile(NitroFileProvider provider, string path, uint offset, uint size, ushort id)
        : this(provider, path, GetName(path), offset, size, id)
    {
    }

    public byte[] Read() => Provider.Extract(this);

    public DataArchive CreateArchive() => Provider.Open(this);

    private static string GetName(string path)
    {
        var slash = path.LastIndexOf('/');
        return slash >= 0 ? path[(slash + 1)..] : path;
    }
}
