using NitroSharp.Core.Data.Archive;
using NitroSharp.Core.Objects.Colors;

namespace NitroSharp.Core.Extensions;

public static class ColorExtensions
{

    public static NitroColor[] ReadColors<TColor>(this DataArchive Ar, int count) where TColor : struct, INitroColorType<TColor>
    {
        var colors = new NitroColor[count];
        for (var i = 0; i < colors.Length; i++)
            colors[i] = TColor.Deserialize(Ar).ToColor();

        return colors;
    }
}
