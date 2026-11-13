using NitroSharp.Core.Data.Archive;

namespace NitroSharp.Core.Objects.Colors;

public interface INitroColorType<TSelf> where TSelf : struct, INitroColorType<TSelf>
{
    static abstract int Size { get; }

    static abstract TSelf Deserialize(DataArchive Ar);

    NitroColor ToColor();
}
