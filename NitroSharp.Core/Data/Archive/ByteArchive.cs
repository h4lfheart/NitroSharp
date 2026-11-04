using System.Runtime.InteropServices;

namespace NitroSharp.Core.Data.Archive;

public sealed class ByteArchive : DataArchive
{
    public override string Name { get; }
    public override long Length { get; }

    private readonly ReadOnlyMemory<byte> _data;

    public ByteArchive(string name, byte[] data)
        : this(name, (ReadOnlyMemory<byte>)data)
    {
    }

    public ByteArchive(string name, ReadOnlyMemory<byte> data)
    {
        ArgumentNullException.ThrowIfNull(name);
        Name = name;
        _data = data;
        Length = data.Length;
    }

    public ReadOnlySpan<byte> GetSpan(int length)
    {
        CheckReadSize(length);
        var span = _data.Span.Slice((int)Position, length);
        Position += length;
        return span;
    }

    public override unsafe T Read<T>()
    {
        var size = sizeof(T);
        CheckReadSize(size);
        var value = MemoryMarshal.Read<T>(_data.Span.Slice((int)Position, size));
        Position += size;
        return value;
    }

    public override void ReadArray<T>(Span<T> destination)
    {
        if (destination.IsEmpty)
            return;

        var bytes = MemoryMarshal.AsBytes(destination);
        CheckReadSize(bytes.Length);
        _data.Span.Slice((int)Position, bytes.Length).CopyTo(bytes);
        Position += bytes.Length;
    }

    public override unsafe T[] ReadArray<T>(int length)
    {
        if (length < 0)
            throw new ArgumentOutOfRangeException(nameof(length));
        if (length == 0)
            return [];

        var size = length * sizeof(T);
        CheckReadSize(size);
        var result = new T[length];
        _data.Span.Slice((int)Position, size).CopyTo(MemoryMarshal.AsBytes(result.AsSpan()));
        Position += size;
        return result;
    }

    public override DataArchive Clone()
        => new ByteArchive(Name, _data) { Position = Position };

    public override DataArchive Slice(long offset, long length)
    {
        ValidateSlice(offset, length);
        return new ByteArchive($"{Name}[{offset}..{offset + length}]", _data.Slice((int)offset, (int)length));
    }

    protected override int ReadAtCore(long position, Span<byte> destination)
    {
        _data.Span.Slice((int)position, destination.Length).CopyTo(destination);
        return destination.Length;
    }
}
