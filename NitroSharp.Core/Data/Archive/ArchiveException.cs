namespace NitroSharp.Core.Data.Archive;

public class ArchiveException(DataArchive Ar, string message, Exception? innerException = null)
    : Exception(Format(Ar, message), innerException)
{
    public DataArchive Archive { get; } = Ar;
    public long Position { get; } = Ar.Position;

    private static string Format(DataArchive Ar, string message)
        => $"[{Ar.Name} @ 0x{Ar.Position:X8}] {message}";
}
