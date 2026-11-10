using System.Diagnostics.CodeAnalysis;
using NitroSharp.Core.Data.Archive;
using NitroSharp.Core.Objects.Files;
using NitroSharp.Core.Objects.Rom;

namespace NitroSharp.Core.Providers;

public class NitroFileProvider : IFileProvider
{
    public NitroRomHeader Header { get; private set; }
    public IReadOnlyDictionary<string, IRomFile> Files => _files;

    private readonly DataArchive _archive;
    
    private readonly Dictionary<string, IRomFile> _files = new(StringComparer.Ordinal);
    private readonly Dictionary<ushort, IRomFile> _filesById = new();
    
    private NitroNameTable _nameTable;
    private NitroDataTable<NitroFatEntry> _fat;
    
    public NitroFileProvider(string path)
        : this(new FileArchive(path))
    {
    }

    public NitroFileProvider(FileInfo file)
        : this(new FileArchive(file))
    {
    }

    public NitroFileProvider(byte[] data)
        : this(new ByteArchive("root", data))
    {
    }

    public NitroFileProvider(ReadOnlyMemory<byte> data)
        : this(new ByteArchive("root", data))
    {
    }

    public NitroFileProvider(DataArchive Ar)
    {
        _archive = Ar;
    }

    public void Initialize()
    {
        _archive.Position = 0;
        
        Header = new NitroRomHeader(_archive);

        _fat = new NitroDataTable<NitroFatEntry>(_archive, Header.FileAllocationTable);
        
        _nameTable = new NitroNameTable(_archive, Header.FileNameTable);
        foreach (var (id, path) in _nameTable.PathsById)
            AddFile(path, id);
    }

    public IRomFile? LoadFile(string path) => _files.GetValueOrDefault(path);

    public IRomFile? LoadFile(ushort id) => _filesById.GetValueOrDefault(id);

    public DataArchive Open(IRomFile file) => _archive.CreateSlice(file.Offset, file.Size);

    private void AddFile(string path, ushort id)
    {
        if (!_fat.TryGet(id, out var entry) || entry.Size == 0)
            return;

        var file = new RomFile(this, path, entry.Start, entry.Size, id);
        _files[path] = file;
        _filesById[id] = file;
    }
}
