using NitroSharp.Core.Objects.Colors;
using NitroSharp.Core.Objects.Exports.Graphics;
using NitroSharp.Conversion.Palettes;

namespace NitroSharp.Conversion.Images;

public static class ImageConverter
{
    public static CIndexedImage Decode(this NitroNCGR ncgr)
        => ImageDecoder.DecodeIndices(ncgr.GraphicData);

    public static CImage ToImage(this CIndexedImage image, CPalette palette, int paletteIndex = 0)
    {
        var colors = palette.Palettes[paletteIndex];

        var pixels = new NitroColor[image.Indices.Length];
        for (var i = 0; i < image.Indices.Length; i++)
        {
            var index = image.Indices[i];
            if (index >= colors.Length)
                continue;

            var color = colors[index];
            pixels[i] = index == 0
                ? new NitroColor(color.Red, color.Green, color.Blue, 0)
                : color;
        }

        return new CImage(image.Width, image.Height, pixels);
    }
}
