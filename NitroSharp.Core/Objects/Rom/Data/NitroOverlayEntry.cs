using System.Runtime.InteropServices;

namespace NitroSharp.Core.Objects.Rom.Data;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly struct NitroOverlayEntry
{
    public readonly uint Id;
    public readonly uint RamAddress;
    public readonly uint RamSize;
    public readonly uint BssSize;
    public readonly uint StaticInitStart;
    public readonly uint StaticInitEnd;
    public readonly uint FileId;
    public readonly uint Reserved;
}
