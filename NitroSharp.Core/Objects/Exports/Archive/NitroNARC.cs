using NitroSharp.Core.Data;
using NitroSharp.Core.Data.Archive;
using NitroSharp.Core.Objects.Exports.Generic;
using NitroSharp.Core.Objects.Files;
using NitroSharp.Core.Providers;

namespace NitroSharp.Core.Objects.Exports.Archive;

public sealed class NitroNARC : NitroObject, INitroDeserializable<NitroNARC>, IFileProvider
{

    public IReadOnlyDictionary<string, IRomFile> Files => _files;

    private readonly NitroAllocationBlock _fat;
    private readonly NitroNameBlock _fnt;
    private readonly NitroImageBlock _img;
    
    private readonly Dictionary<string, IRomFile> _files = new(StringComparer.Ordinal);
    private readonly Dictionary<ushort, IRomFile> _filesById = new();

    private NitroNARC(DataArchive Ar) : base(Ar, "NARC")
    {
        _fat = ReadChunk<NitroAllocationBlock>(Ar, "BTAF");
        _fnt = ReadChunk<NitroNameBlock>(Ar, "BTNF");
        _img = ReadChunk<NitroImageBlock>(Ar, "GMIF");

        for (ushort id = 0; id < _fat.Entries.Count; id++)
        {
            if (!_fat.Entries.TryGet(id, out var entry) || entry.Size == 0)
                continue;
            
            var path = _fnt.Table.PathsById.GetValueOrDefault(id, string.Empty);
            var romFile = new RomFile(this, path, path, entry.Start, entry.Size, id);
            _filesById[id] = romFile;

            if (path.Length > 0)
                _files[path] = romFile;
        }
    }

    public IRomFile? LoadFile(string path) => _files.GetValueOrDefault(path);

    public IRomFile? LoadFile(ushort id) => _filesById.GetValueOrDefault(id);

    public DataArchive Open(IRomFile file) => _img.Open(file.Offset, file.Size);

    public static NitroNARC Deserialize(DataArchive Ar) => new(Ar);
}
