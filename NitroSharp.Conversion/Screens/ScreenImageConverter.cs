using NitroSharp.Core.Objects.Colors;
using NitroSharp.Conversion.Images;
using NitroSharp.Conversion.Palettes;

namespace NitroSharp.Conversion.Screens;

public static class ScreenImageConverter
{
    public static CImage ToScreenImage(this CScreen screen, CIndexedImage graphic, CPalette palette)
    {
        var screenTilesX = screen.Width / 8;
        var canvasTilesX = graphic.Width / 8;
        var canvasTileCount = canvasTilesX * (graphic.Height / 8);

        var pixels = new NitroColor[screen.Width * screen.Height];

        var banks = graphic.ColorDepth switch
        {
            NitroColorDepth.Bpp4 => palette.Palettes,
            NitroColorDepth.Bpp8 => [palette.Palettes.SelectMany(colors => colors).ToArray()],
            _ => throw new NotSupportedException($"Unsupported color depth {graphic.ColorDepth}."),
        };

        for (var i = 0; i < screen.Entries.Length; i++)
        {
            var tileX = (i % screenTilesX) * 8;
            var tileY = (i / screenTilesX) * 8;
            if (tileY >= screen.Height)
                continue;

            var entry = screen.Entries[i];
            if (entry.TileIndex >= canvasTileCount)
                continue;

            var srcTileX = (entry.TileIndex % canvasTilesX) * 8;
            var srcTileY = (entry.TileIndex / canvasTilesX) * 8;

            for (var y = 0; y < 8; y++)
            {
                for (var x = 0; x < 8; x++)
                {
                    var pixel = graphic.Indices[(srcTileY + (entry.VerticalFlip ? 7 - y : y)) * graphic.Width + srcTileX + (entry.HorizontalFlip ? 7 - x : x)];
                    if (pixel == 0)
                        continue;

                    var bank = graphic.ColorDepth switch
                    {
                        NitroColorDepth.Bpp4 => entry.PaletteIndex,
                        NitroColorDepth.Bpp8 => 0,
                        _ => throw new NotSupportedException($"Unsupported color depth {graphic.ColorDepth}."),
                    };
                    
                    if (bank >= banks.Length)
                        continue;

                    var colors = banks[bank];
                    if (pixel >= colors.Length)
                        continue;

                    pixels[(tileY + y) * screen.Width + tileX + x] = colors[pixel];
                }
            }
        }

        return new CImage(screen.Width, screen.Height, pixels);
    }
}
