using NitroSharp.Core.Data;
using NitroSharp.Core.Data.Archive;

namespace NitroSharp.Core.Objects.Exports.Palettes;

public sealed class NitroPaletteIndexBlock : INitroDeserializable<NitroPaletteIndexBlock>
{
    public ushort PaletteCount { get; }
    public uint TableOffset { get;  }
    public ushort[] Indices { get; }

    public NitroPaletteIndexBlock(DataArchive Ar)
    {
        PaletteCount = Ar.Read<ushort>();

        Ar.Position += sizeof(ushort);

        TableOffset = Ar.Read<uint>();
        Indices = Ar.ReadArray<ushort>(PaletteCount);
    }

    public static NitroPaletteIndexBlock Deserialize(DataArchive Ar) => new(Ar);
}
