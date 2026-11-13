namespace NitroSharp.Conversion.Screens;

public readonly record struct CScreenEntry(ushort Value)
{
    public int TileIndex => Value & 0b11_1111_1111;
    public bool HorizontalFlip => (Value & 0b0100_0000_0000) != 0;
    public bool VerticalFlip => (Value & 0b1000_0000_0000) != 0;
    public int PaletteIndex => (Value >> 12) & 0b1111;
}

public sealed class CScreen
{
    public int Width { get; }
    public int Height { get; }
    public CScreenEntry[] Entries { get; }

    public CScreen(int width, int height, ushort[] entries)
    {
        Width = width;
        Height = height;
        Entries = [..entries.Select(entry => new CScreenEntry(entry))];
    }
}
