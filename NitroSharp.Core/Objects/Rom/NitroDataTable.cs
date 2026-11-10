using System.Collections;
using NitroSharp.Core.Data.Archive;

namespace NitroSharp.Core.Objects.Rom;

public sealed class NitroDataTable<T> : IReadOnlyList<T> where T : unmanaged
{
    public int Count { get; }

    private readonly T[] _entries;

    public unsafe NitroDataTable(DataArchive Ar, NitroTableInfo table)
    {
        var count = (int)(table.Size / (uint)sizeof(T));
        Ar.Position = table.Offset;
        _entries = Ar.ReadArray<T>(count);
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
