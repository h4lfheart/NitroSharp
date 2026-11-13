using NitroSharp.Core.Objects.Colors;

namespace NitroSharp.Conversion.Images;

public sealed class CImage
{
    public int Width { get; }
    public int Height { get; }
    public NitroColor[] Pixels { get; }

    public CImage(int width, int height, NitroColor[] pixels)
    {
        Width = width;
        Height = height;
        Pixels = pixels;
    }
}
