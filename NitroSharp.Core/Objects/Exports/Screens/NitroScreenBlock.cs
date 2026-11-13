using NitroSharp.Core.Data;
using NitroSharp.Core.Data.Archive;

namespace NitroSharp.Core.Objects.Exports.Screens;

public sealed class NitroScreenBlock : INitroDeserializable<NitroScreenBlock>
{
    public ushort Width { get; }
    public ushort Height { get; }
    public uint DataSize { get; }
    public ushort[] Entries { get; }

    public NitroScreenBlock(DataArchive Ar)
    {
        Width = Ar.Read<ushort>();
        Height = Ar.Read<ushort>();

        Ar.Position += sizeof(uint);

        DataSize = Ar.Read<uint>();

        Entries = Ar.ReadArray<ushort>((int)(DataSize / sizeof(ushort)));
    }

    public static NitroScreenBlock Deserialize(DataArchive Ar) => new(Ar);
}
