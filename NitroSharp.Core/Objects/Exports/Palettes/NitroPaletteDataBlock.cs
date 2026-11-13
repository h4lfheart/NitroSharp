using NitroSharp.Core.Data;
using NitroSharp.Core.Data.Archive;
using NitroSharp.Core.Extensions;
using NitroSharp.Core.Objects.Colors;
using NitroSharp.Core.Objects.Colors.Types;

namespace NitroSharp.Core.Objects.Exports.Palettes;

public sealed class NitroPaletteDataBlock : INitroDeserializable<NitroPaletteDataBlock>
{
    public NitroColorDepth ColorDepth { get; }
    public int ColorsPerPalette { get; }
    public uint DataSize { get; }
    public uint DataOffset { get; }
    public NitroColor[] Colors { get; }

    public int PaletteCount => Colors.Length / ColorsPerPalette;

    public NitroPaletteDataBlock(DataArchive Ar)
    {
        ColorDepth = Ar.ReadEnum<NitroColorDepth>();

        ColorsPerPalette = ColorDepth switch
        {
            NitroColorDepth.Bpp4 => 16,
            NitroColorDepth.Bpp8 => 256,
            _ => throw new ArchiveException(Ar, $"Invalid palette color depth 0x{(uint)ColorDepth:X8}."),
        };

        Ar.Position += sizeof(uint);

        DataSize = Ar.Read<uint>();
        DataOffset = Ar.Read<uint>();

        Colors = Ar.ReadColors<BGR555>((int)(DataSize / BGR555.Size));
    }

    public static NitroPaletteDataBlock Deserialize(DataArchive Ar) => new(Ar);
}
