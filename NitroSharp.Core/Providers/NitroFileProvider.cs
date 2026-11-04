using System.Diagnostics.CodeAnalysis;
using NitroSharp.Core.Data.Archive;
using NitroSharp.Core.Objects.Rom;
using NitroSharp.Core.Objects.Rom.Data;

namespace NitroSharp.Core.Providers;

public class NitroFileProvider : IFileProvider
{
    public NitroRomHeader Header { get; }
    public IReadOnlyDictionary<string, IRomFile> Files => _files;

    private readonly DataArchive _archive;
    
    private readonly Dictionary<string, IRomFile> _files = new(StringComparer.Ordinal);
    private readonly Dictionary<string, IRomFile>.AlternateLookup<ReadOnlySpan<char>> _fileLookup;
    
    private readonly NitroNameTable _nameTable;
    private readonly NitroDataTable<NitroFatEntry> _fat;
    private readonly NitroDataTable<NitroOverlayEntry> _arm9Overlays;
    private readonly NitroDataTable<NitroOverlayEntry> _arm7Overlays;

    public NitroFileProvider(DataArchive archive)
    {
        _archive = archive;

        archive.Position = 0;
        Header = new NitroRomHeader(_archive);

        _fat = new NitroDataTable<NitroFatEntry>(_archive, Header.FileAllocationTable);
        
        _nameTable = new NitroNameTable(_archive, Header.FileNameTable);
        foreach (var (id, path) in _nameTable.PathsById)
            AddFile(path, id);

        _arm9Overlays = new NitroDataTable<NitroOverlayEntry>(_archive, Header.Arm9Overlay);
        foreach (var overlay in _arm9Overlays)
            AddFile($"overlay9/{overlay.Id}", (ushort) overlay.FileId);

        _arm7Overlays = new NitroDataTable<NitroOverlayEntry>(_archive, Header.Arm7Overlay);
        foreach (var overlay in _arm7Overlays)
            AddFile($"overlay7/{overlay.Id}", (ushort) overlay.FileId);

        _fileLookup = _files.GetAlternateLookup<ReadOnlySpan<char>>();
    }

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

    public bool TryGetFile(ReadOnlySpan<char> path, [NotNullWhen(true)] out IRomFile? file)
        => _fileLookup.TryGetValue(path, out file);

    internal byte[] Extract(IRomFile file)
        => _archive.ReadBytesAt(file.Offset, checked((int) file.Size));

    internal DataArchive Open(IRomFile file)
        => _archive.CreateSlice(file.Offset, file.Size);

    private void AddFile(string path, ushort id)
    {
        if (!_fat.TryGet(id, out var entry) || entry.Size == 0)
            return;

        _files[path] = new RomFile(this, path, entry.Start, entry.Size, id);
    }
}
