using System.Collections;
using NitroSharp.Core.Data.Archive;

namespace NitroSharp.Core.Objects.Rom.Data;

public sealed class NitroDataTable<T> : IReadOnlyList<T> where T : unmanaged
{
    public int Count { get; }

    private readonly T[] _entries;

    public unsafe NitroDataTable(DataArchive ar, NitroTableInfo table)
    {
        var count = (int)(table.Size / (uint)sizeof(T));
        ar.Position = table.Offset;
        _entries = ar.ReadArray<T>(count);
        Count = count;
    }

    public T this[int index] => _entries[index];

    public bool TryGet(int index, out T entry)
    {
        entry = default;
        if (index < 0 || index >= Count)
            return false;

        entry = _entries[index];
        return true;
    }

    public IEnumerator<T> GetEnumerator() => ((IReadOnlyList<T>)_entries).GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
