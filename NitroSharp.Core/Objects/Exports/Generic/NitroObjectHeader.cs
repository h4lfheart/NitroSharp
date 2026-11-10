using NitroSharp.Core.Data.Archive;

namespace NitroSharp.Core.Objects.Exports.Generic;

public enum NitroByteOrder : ushort
{
    LittleEndian = 0xFFFE,
    BigEndian = 0xFEFF,
}

public sealed class NitroObjectHeader
{
    public string Magic { get; }
    public NitroByteOrder BOM { get; }
    public Version Version { get; }
    public uint FileSize { get; }
    public ushort HeaderSize { get; }
    public ushort SectionCount { get; }

    public NitroObjectHeader(DataArchive Ar, string expectedMagic)
    {
        Magic = Ar.ReadString(4);
        if (!string.Equals(Magic, expectedMagic, StringComparison.Ordinal))
            throw new ArchiveException(Ar, $"Expected Nitro Magic '{expectedMagic}', got '{Magic}'.");

        BOM = (NitroByteOrder)Ar.Read<ushort>();
        if (BOM != NitroByteOrder.LittleEndian)
            throw new ArchiveException(Ar, $"Invalid Byte Order Marker 0x{(ushort)BOM:X4}.");

        var version = Ar.Read<ushort>();
        Version = new Version(version >> 8, version & 0xFF);

        FileSize = Ar.Read<uint>();
        HeaderSize = Ar.Read<ushort>();
        SectionCount = Ar.Read<ushort>();
    }
}
