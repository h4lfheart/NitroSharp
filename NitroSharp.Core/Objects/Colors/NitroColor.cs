namespace NitroSharp.Core.Objects.Colors;

public readonly struct NitroColor
{
    public byte Red { get; }
    public byte Green { get; }
    public byte Blue { get; }
    public byte Alpha { get; }
    
    public string Hex => $"#{Red:X2}{Green:X2}{Blue:X2}{Alpha:X2}";

    public NitroColor(byte red, byte green, byte blue, byte alpha = 0xFF)
    {
        Red = red;
        Green = green;
        Blue = blue;
        Alpha = alpha;
    }


    public override string ToString() => $"R: {Red} | G: {Green} | B: {Blue} | A: {Alpha} | Hex: {Hex}";
}
