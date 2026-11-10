using NitroSharp.Core.Data;
using NitroSharp.Core.Data.Archive;
using NitroSharp.Core.Objects.Rom;

namespace NitroSharp.Core.Objects.Exports.Archive;

public sealed class NitroNameBlock : INitroDeserializable<NitroNameBlock>
{
    public NitroNameTable Table { get; }

    public NitroNameBlock(DataArchive Ar)
    {
        Table = new NitroNameTable(Ar, new NitroTableInfo(0, (uint)Ar.Length));
    }

    public static NitroNameBlock Deserialize(DataArchive Ar) => new(Ar);
}
