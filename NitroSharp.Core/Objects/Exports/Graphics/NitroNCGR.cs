using NitroSharp.Core.Data;
using NitroSharp.Core.Data.Archive;
using NitroSharp.Core.Objects.Exports.Generic;

namespace NitroSharp.Core.Objects.Exports.Graphics;

public sealed class NitroNCGR : NitroObject, INitroDeserializable<NitroNCGR>
{
    public NitroGraphicDataBlock GraphicData { get; }
    public NitroGraphicPositionBlock? GraphicPosition { get; }

    private const string Magic = "RGCN";

    private NitroNCGR(DataArchive Ar) : base(Ar, Magic)
    {
        GraphicData = ReadChunk<NitroGraphicDataBlock>(Ar, "RAHC");

        if (Header.SectionCount == 2)
            GraphicPosition = ReadChunk<NitroGraphicPositionBlock>(Ar, "SOPC");
    }

    public static NitroNCGR Deserialize(DataArchive Ar) => new(Ar);
}
