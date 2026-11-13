using NitroSharp.Core.Objects.Colors;

namespace NitroSharp.Conversion.Palettes;

public sealed class CPalette
{
    public NitroColor[][] Palettes { get; }

    public CPalette(NitroColor[][] palettes)
    {
        Palettes = palettes;
    }
}
