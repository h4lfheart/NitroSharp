using NitroSharp.Core.Data;
using NitroSharp.Core.Data.Archive;

namespace NitroSharp.Core.Objects.Exports.Generic;

public abstract class NitroObject
{
    public NitroObjectHeader Header { get; }

    protected NitroObject(DataArchive Ar, string expectedMagic)
    {
        Header = new NitroObjectHeader(Ar, expectedMagic);
    }

    protected static T ReadChunk<T>(DataArchive Ar, string expectedMagic) where T : INitroDeserializable<T>
    {
        var magic = Ar.ReadString(4);
        if (!string.Equals(magic, expectedMagic, StringComparison.Ordinal))
            throw new ArchiveException(Ar, $"Expected Nitro chunk '{expectedMagic}', got '{magic}'.");

        var size = Ar.Read<uint>();
        var chunk = Ar.CreateSlice(Ar.Position, size - sizeof(uint) * 2);
        Ar.Position += size - sizeof(uint) * 2;
        return T.Deserialize(chunk);
    }
}
