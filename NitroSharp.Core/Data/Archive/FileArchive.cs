using Microsoft.Win32.SafeHandles;

namespace NitroSharp.Core.Data.Archive;

public sealed class FileArchive : DataArchive
{
    public override string Name { get; }
    public override long Length { get; }

    private readonly SafeFileHandle _fileHandle;
    private readonly bool _ownsHandle;

    public FileArchive(string filePath)
        : this(
            filePath,
            File.OpenHandle(
                filePath,
                FileMode.Open,
                FileAccess.Read,
                FileShare.ReadWrite | FileShare.Delete,
                FileOptions.Asynchronous),
            ownsHandle: true)
    {
    }

    public FileArchive(FileInfo fileInfo)
        : this(fileInfo.FullName)
    {
    }

    public FileArchive(string name, SafeFileHandle fileHandle, bool ownsHandle = true)
    {
        ArgumentNullException.ThrowIfNull(name);
        ArgumentNullException.ThrowIfNull(fileHandle);
        if (fileHandle.IsInvalid)
            throw new ArgumentException("File handle is invalid.", nameof(fileHandle));

        Name = name;
        _fileHandle = fileHandle;
        _ownsHandle = ownsHandle;
        Length = RandomAccess.GetLength(fileHandle);
    }

    public override DataArchive Clone()
        => new FileArchive(Name) { Position = Position };

    public override DataArchive Slice(long offset, long length)
    {
        ValidateSlice(offset, length);
        return new SlicedDataArchive(this, offset, length);
    }

    protected override int ReadAtCore(long position, Span<byte> destination)
        => RandomAccess.Read(_fileHandle, destination, position);

    protected override void Dispose(bool disposing)
    {
        if (disposing && _ownsHandle && !_fileHandle.IsClosed)
            _fileHandle.Dispose();
    }
}
