namespace NitroSharp.Conversion.Images;

public sealed class CIndexedImage
{
    public int Width { get; }
    public int Height { get; }
    public byte[] Indices { get; }

    public CIndexedImage(int width, int height, byte[] indices)
    {
        Width = width;
        Height = height;
        Indices = indices;
    }
}
