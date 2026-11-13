using NitroSharp.Core.Data;
using NitroSharp.Core.Data.Archive;
using NitroSharp.Core.Objects.Colors;

namespace NitroSharp.Core.Objects.Exports.Graphics;

public sealed class NitroGraphicDataBlock : INitroDeserializable<NitroGraphicDataBlock>
{
    public ushort TileHeight { get; }
    public ushort TileWidth { get; }
    public NitroColorDepth ColorDepth { get; }
    public ushort CharacterFormat { get; }
    public uint Flags { get; }
    public uint DataSize { get; }
    public byte[] TileData { get; }

    public NitroGraphicDataBlock(DataArchive Ar)
    {
        TileHeight = Ar.Read<ushort>();
        TileWidth = Ar.Read<ushort>();
        ColorDepth = Ar.ReadEnum<NitroColorDepth>();

        CharacterFormat = Ar.Read<ushort>();
        Ar.Position += sizeof(ushort);
        Flags = Ar.Read<uint>();

        DataSize = Ar.Read<uint>();

        Ar.Position += sizeof(uint);
        TileData = Ar.ReadBytes((int)DataSize);
    }

    public static NitroGraphicDataBlock Deserialize(DataArchive Ar) => new(Ar);
}
