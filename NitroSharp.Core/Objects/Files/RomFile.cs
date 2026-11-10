using NitroSharp.Core.Data;
using NitroSharp.Core.Data.Archive;
using NitroSharp.Core.Extensions;
using NitroSharp.Core.Providers;

namespace NitroSharp.Core.Objects.Files;

public class RomFile(IFileProvider provider, string path, string name, uint offset, uint size, ushort id) : IRomFile
{
    public IFileProvider Provider { get; } = provider;

    public string Path { get; } = path;
    public string Name { get; } = name;
    public uint Offset { get; } = offset;
    public uint Size { get; } = size;
    public ushort Id { get; } = id;

    public RomFile(IFileProvider provider, string path, uint offset, uint size, ushort id)
        : this(provider, path, path.SubstringBeforeLast('/'), offset, size, id)
    {
    }

    public DataArchive Open() => Provider.Open(this);
    
    public T Load<T>() where T : INitroDeserializable<T>
    {
        using var archive = Open();
        return T.Deserialize(archive);
    }
}
