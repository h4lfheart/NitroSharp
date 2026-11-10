using NitroSharp.Core.Data;
using NitroSharp.Core.Data.Archive;

namespace NitroSharp.Core.Objects.Exports.Archive;

public sealed class NitroImageBlock : INitroDeserializable<NitroImageBlock>
{
    public DataArchive Archive { get; }

    public NitroImageBlock(DataArchive Ar)
    {
        Archive = Ar;
    }

    public DataArchive Open(uint offset, uint size) => Archive.CreateSlice(offset, size);

    public static NitroImageBlock Deserialize(DataArchive Ar) => new(Ar);
}
