using NitroSharp.Core.Data;
using NitroSharp.Core.Data.Archive;
using NitroSharp.Core.Objects.Exports.Generic;

namespace NitroSharp.Core.Objects.Exports.Screens;

public sealed class NitroNSCR : NitroObject, INitroDeserializable<NitroNSCR>
{
    public NitroScreenBlock ScreenData { get; }

    private const string Magic = "RCSN";

    private NitroNSCR(DataArchive Ar) : base(Ar, Magic)
    {
        ScreenData = ReadChunk<NitroScreenBlock>(Ar, "NRCS");
    }

    public static NitroNSCR Deserialize(DataArchive Ar) => new(Ar);
}
