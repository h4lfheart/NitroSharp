using NitroSharp.Core.Data;
using NitroSharp.Core.Data.Archive;
using NitroSharp.Core.Objects.Exports.Generic;

namespace NitroSharp.Core.Objects.Exports.Palettes;

public sealed class NitroNCLR : NitroObject, INitroDeserializable<NitroNCLR>
{
    public NitroPaletteDataBlock PaletteData { get; }
    public NitroPaletteIndexBlock? PaletteIndices { get; }

    private const string Magic = "RLCN";

    private NitroNCLR(DataArchive Ar) : base(Ar, Magic)
    {
        PaletteData = ReadChunk<NitroPaletteDataBlock>(Ar, "TTLP");

        if (Header.SectionCount == 2)
            PaletteIndices = ReadChunk<NitroPaletteIndexBlock>(Ar, "PMCP");
    }

    public static NitroNCLR Deserialize(DataArchive Ar) => new(Ar);
}
