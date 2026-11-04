using System.Runtime.InteropServices;

namespace NitroSharp.Core.Objects.Rom.Data;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly struct NitroFatEntry
{
    public readonly uint Start;
    public readonly uint End;

    public uint Size => End - Start;
}
