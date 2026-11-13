using NitroSharp.Core.Data;
using NitroSharp.Core.Data.Archive;

namespace NitroSharp.Core.Objects.Exports.Graphics;

public sealed class NitroGraphicPositionBlock : INitroDeserializable<NitroGraphicPositionBlock>
{
    public ushort X { get; }
    public ushort Y { get; }
    public ushort TileWidth { get; }
    public ushort TileHeight { get; }

    public NitroGraphicPositionBlock(DataArchive Ar)
    {
        X = Ar.Read<ushort>();
        Y = Ar.Read<ushort>();
        TileWidth = Ar.Read<ushort>();
        TileHeight = Ar.Read<ushort>();
    }

    public static NitroGraphicPositionBlock Deserialize(DataArchive Ar) => new(Ar);
}
