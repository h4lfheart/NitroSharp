using System.Runtime.InteropServices;
using NitroSharp.Core.Data.Archive;

namespace NitroSharp.Core.Objects.Rom.Data;

public sealed class NitroNameTable
{
    public ushort FirstFileId { get; }
    public Dictionary<ushort, string> PathsById { get; } = new();

    private const ushort RootId = 0xF000;

    private readonly long _startOffset;

    public NitroNameTable(DataArchive Ar, NitroTableInfo table)
    {
        _startOffset = table.Offset;

        var root = ReadDirectoryEntry(Ar, RootId);
        FirstFileId = root.FirstFileId;
        Walk(Ar, RootId, path: "");
    }

    private void Walk(DataArchive Ar, ushort directoryId, string path)
    {
        var entry = ReadDirectoryEntry(Ar, directoryId);
        Ar.Position = _startOffset + entry.SubTableOffset;

        var fileId = entry.FirstFileId;
        var children = new List<(ushort Id, string Path)>();

        while (true)
        {
            var control = Ar.Read<byte>();
            if (control == 0)
                break;

            var name = Ar.ReadString(control & 0b01111111);
            var fullPath = path.Length == 0 ? name : $"{path}/{name}";

            if ((control & 0b10000000) == 0)
            {
                PathsById[fileId++] = fullPath;
            }
            else
            {
                children.Add((Ar.Read<ushort>(), fullPath));
            }
        }

        foreach (var (childId, childPath) in children)
            Walk(Ar, childId, childPath);
    }

    private unsafe NitroDirectoryEntry ReadDirectoryEntry(DataArchive Ar, ushort directoryId)
    {
        Ar.Position = _startOffset + (directoryId & 0xFFF) * sizeof(NitroDirectoryEntry);
        return Ar.Read<NitroDirectoryEntry>();
    }
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly struct NitroDirectoryEntry
{
    public readonly uint SubTableOffset;
    public readonly ushort FirstFileId;
    public readonly ushort ParentOrCount;
}
