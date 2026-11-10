using System.Runtime.CompilerServices;
using NitroSharp.Core.Data;
using NitroSharp.Core.Data.Archive;
using NitroSharp.Core.Objects.Rom;

namespace NitroSharp.Core.Objects.Exports.Archive;

public sealed class NitroAllocationBlock : INitroDeserializable<NitroAllocationBlock>
{
    public ushort FileCount { get; }
    public NitroDataTable<NitroFatEntry> Entries { get; }

    public NitroAllocationBlock(DataArchive Ar)
    {
        FileCount = Ar.Read<ushort>();

        Ar.Position += 2;
        
        Entries = new NitroDataTable<NitroFatEntry>(Ar, new NitroTableInfo((uint)Ar.Position, (uint)(FileCount * Unsafe.SizeOf<NitroFatEntry>())));
    }

    public static NitroAllocationBlock Deserialize(DataArchive Ar) => new(Ar);
}
