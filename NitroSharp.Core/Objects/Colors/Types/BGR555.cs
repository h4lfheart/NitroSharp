using NitroSharp.Core.Data.Archive;

namespace NitroSharp.Core.Objects.Colors.Types;

public readonly struct BGR555 : INitroColorType<BGR555>
{
    public readonly ushort Value;

    public static int Size => sizeof(ushort);

    public BGR555(ushort value) => Value = value;

    public static BGR555 Deserialize(DataArchive Ar) => new(Ar.Read<ushort>());

    public NitroColor ToColor()
    {
        var red = Expand(Value & 0x1F);
        var green = Expand((Value >> 5) & 0x1F);
        var blue = Expand((Value >> 10) & 0x1F);
        return new NitroColor(red, green, blue);
    }

    private static byte Expand(int value) => (byte)((value << 3) | (value >> 2));
}
