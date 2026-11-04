using System.Buffers;
using System.Runtime.InteropServices;
using System.Text;

namespace NitroSharp.Core.Data.Archive;

public abstract class DataArchive : IDisposable
{
    public abstract string Name { get; }
    public abstract long Length { get; }
    public long Position { get; set; }

    private const int StackallocThreshold = 512;

    public int Read(Span<byte> destination)
    {
        var read = ReadAt(Position, destination);
        Position += read;
        return read;
    }

    public int ReadAt(long position, Span<byte> destination)
    {
        if (destination.IsEmpty)
            return 0;

        CheckReadSize(position, destination.Length);

        var total = 0;
        while (total < destination.Length)
        {
            var read = ReadAtCore(position + total, destination[total..]);
            if (read <= 0)
                throw new EndOfStreamException(
                    $"Expected to read {destination.Length} bytes at {position}, got {total}.");

            total += read;
        }

        return total;
    }

    public byte[] ReadBytes(int length)
    {
        var buffer = new byte[length];
        Read(buffer);
        return buffer;
    }

    public byte[] ReadBytesAt(long position, int length)
    {
        var buffer = new byte[length];
        ReadAt(position, buffer);
        return buffer;
    }

    public virtual unsafe T Read<T>() where T : unmanaged
    {
        var size = sizeof(T);
        CheckReadSize(size);

        if (size <= StackallocThreshold)
        {
            Span<byte> buffer = stackalloc byte[size];
            Read(buffer);
            return MemoryMarshal.Read<T>(buffer);
        }

        var rented = ArrayPool<byte>.Shared.Rent(size);
        try
        {
            var buffer = rented.AsSpan(0, size);
            Read(buffer);
            return MemoryMarshal.Read<T>(buffer);
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(rented);
        }
    }

    public virtual T Peek<T>() where T : unmanaged
    {
        var saved = Position;
        try
        {
            return Read<T>();
        }
        finally
        {
            Position = saved;
        }
    }

    public void Peek(Action action)
    {
        var saved = Position;
        try
        {
            action();
        }
        finally
        {
            Position = saved;
        }
    }

    public T Peek<T>(Func<T> getter)
    {
        var saved = Position;
        try
        {
            return getter();
        }
        finally
        {
            Position = saved;
        }
    }

    public virtual T[] ReadArray<T>(int length) where T : unmanaged
    {
        if (length < 0)
            throw new ArgumentOutOfRangeException(nameof(length));
        if (length == 0)
            return [];

        var result = new T[length];
        ReadArray(result.AsSpan());
        return result;
    }

    public virtual void ReadArray<T>(Span<T> destination) where T : unmanaged
    {
        if (destination.IsEmpty)
            return;

        Read(MemoryMarshal.AsBytes(destination));
    }

    public T[] ReadArray<T>(int length, Func<T> getter)
    {
        ArgumentNullException.ThrowIfNull(getter);
        if (length < 0)
            throw new ArgumentOutOfRangeException(nameof(length));
        if (length == 0)
            return [];

        var result = new T[length];
        for (var i = 0; i < length; i++)
            result[i] = getter();

        return result;
    }

    public void ReadArray<T>(T[] array, Func<T> getter)
    {
        ArgumentNullException.ThrowIfNull(array);
        ArgumentNullException.ThrowIfNull(getter);

        for (var i = 0; i < array.Length; i++)
            array[i] = getter();
    }

    public string ReadString(int length, Encoding? encoding = null)
    {
        if (length < 0)
            throw new ArgumentOutOfRangeException(nameof(length));
        if (length == 0)
            return string.Empty;

        encoding ??= Encoding.ASCII;
        Span<byte> buffer = length <= StackallocThreshold ? stackalloc byte[length] : new byte[length];
        Read(buffer);

        var nullIndex = buffer.IndexOf((byte)0);
        if (nullIndex >= 0)
            buffer = buffer[..nullIndex];

        return encoding.GetString(buffer);
    }

    public long Seek(long offset, SeekOrigin origin)
    {
        Position = origin switch
        {
            SeekOrigin.Begin => offset,
            SeekOrigin.Current => Position + offset,
            SeekOrigin.End => Length + offset,
            _ => throw new ArgumentOutOfRangeException(nameof(origin), origin, null)
        };
        return Position;
    }

    public void CheckReadSize(int length) => CheckReadSize(Position, length);

    public void CheckReadSize(long position, int length)
    {
        if (length < 0)
            throw new ArgumentOutOfRangeException(nameof(length), length, "Length cannot be negative.");
        if (position < 0 || position > Length)
            throw new ArgumentOutOfRangeException(nameof(position), position, $"Position is outside archive '{Name}'.");
        if (position + length > Length)
            throw new EndOfStreamException(
                $"Cannot read {length} bytes at {position} from '{Name}' (length {Length}).");
    }

    public abstract DataArchive Clone();

    public virtual DataArchive Slice(long offset, long length)
    {
        ValidateSlice(offset, length);
        var bytes = ReadBytesAt(offset, checked((int)length));
        return new ByteArchive($"{Name}[{offset}..{offset + length}]", bytes);
    }

    public DataArchive CreateSlice(long offset, long length)
    {
        ValidateSlice(offset, length);
        return new SlicedDataArchive(this, offset, length);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected abstract int ReadAtCore(long position, Span<byte> destination);

    protected void ValidateSlice(long offset, long length)
    {
        if (offset < 0 || length < 0 || offset + length > Length)
            throw new ArgumentOutOfRangeException(
                $"Invalid slice [{offset}..{offset + length}] for '{Name}' (length {Length}).");
    }

    protected virtual void Dispose(bool disposing)
    {
    }

    protected sealed class SlicedDataArchive : DataArchive
    {
        public override string Name { get; }
        public override long Length { get; }

        private readonly DataArchive _inner;
        private readonly long _baseOffset;
        private readonly bool _ownsInner;

        public SlicedDataArchive(DataArchive inner, long offset, long length, bool ownsInner = false)
        {
            ArgumentNullException.ThrowIfNull(inner);
            inner.ValidateSlice(offset, length);

            _inner = inner;
            _baseOffset = offset;
            _ownsInner = ownsInner;
            Name = $"{inner.Name}[{offset}..{offset + length}]";
            Length = length;
        }

        public override DataArchive Clone()
            => new SlicedDataArchive(_inner.Clone(), _baseOffset, Length, ownsInner: true) { Position = Position };

        public override DataArchive Slice(long offset, long length)
        {
            ValidateSlice(offset, length);
            return new SlicedDataArchive(_inner, _baseOffset + offset, length);
        }

        protected override int ReadAtCore(long position, Span<byte> destination)
            => _inner.ReadAt(_baseOffset + position, destination);

        protected override void Dispose(bool disposing)
        {
            if (disposing && _ownsInner)
                _inner.Dispose();
        }
    }
}
