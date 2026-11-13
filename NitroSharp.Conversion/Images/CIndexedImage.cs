using NitroSharp.Core.Objects.Colors;

namespace NitroSharp.Conversion.Images;

public sealed class CIndexedImage
{
    public int Width { get; }
    public int Height { get; }
    public byte[] Indices { get; }
    public NitroColorDepth ColorDepth { get; }

    public CIndexedImage(int width, int height, byte[] indices, NitroColorDepth colorDepth)
    {
        Width = width;
        Height = height;
        Indices = indices;
        ColorDepth = colorDepth;
    }
}
