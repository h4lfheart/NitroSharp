using NitroSharp.Conversion.Images;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace NitroSharp.Conversion.Images;

public static class ImageExtensions
{
    public static Image<Rgba32> ToImage(this CImage image)
    {
        var result = new Image<Rgba32>(image.Width, image.Height);
        result.ProcessPixelRows(accessor =>
        {
            for (var y = 0; y < accessor.Height; y++)
            {
                var source = image.Pixels.AsSpan(y * image.Width, image.Width);
                var row = accessor.GetRowSpan(y);
                for (var x = 0; x < row.Length; x++)
                {
                    var color = source[x];
                    row[x] = new Rgba32(color.Red, color.Green, color.Blue, color.Alpha);
                }
            }
        });

        return result;
    }
}
