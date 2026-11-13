using NitroSharp.Core.Objects.Colors;
using NitroSharp.Core.Objects.Exports.Palettes;

namespace NitroSharp.Conversion.Palettes;

public static class PaletteConverter
{
    public static CPalette Decode(this NitroNCLR nclr)
    {
        var block = nclr.PaletteData;

        var palettes = new NitroColor[block.PaletteCount][];
        for (var i = 0; i < palettes.Length; i++)
        {
            var palette = new NitroColor[block.ColorsPerPalette];
            Array.Copy(block.Colors, i * block.ColorsPerPalette, palette, 0, block.ColorsPerPalette);
            palettes[i] = palette;
        }

        return new CPalette(palettes);
    }
}
