using NitroSharp.Core.Data.Archive;

namespace NitroSharp.Core.Objects.Rom;

public sealed class NitroRomHeader
{
    public string GameTitle { get; }
    public string GameCode { get; }
    public string MakerCode { get; }
    public NitroUnitCode UnitCode { get; }
    public byte EncryptionSeedSelect { get; }
    public byte DeviceCapacity { get; }
    public NitroRegion Region { get; }
    public byte RomVersion { get; }
    public NitroAutostart Autostart { get; }
    public ArmBinaryInfo Arm9 { get; }
    public ArmBinaryInfo Arm7 { get; }
    public NitroTableInfo FileNameTable { get; }
    public NitroTableInfo FileAllocationTable { get; }
    public NitroTableInfo Arm9Overlay { get; }
    public NitroTableInfo Arm7Overlay { get; }
    public uint NormalCommandSetting { get; }
    public uint Key1CommandSetting { get; }
    public uint IconTitleOffset { get; }
    public ushort SecureAreaChecksum { get; }
    public ushort SecureAreaDelay { get; }
    public uint Arm9AutoLoadHook { get; }
    public uint Arm7AutoLoadHook { get; }
    public ulong SecureAreaDisable { get; }
    public uint TotalUsedRomSize { get; }
    public uint HeaderSize { get; }
    public byte[] NintendoLogo { get; }
    public ushort LogoChecksum { get; }
    public ushort HeaderChecksum { get; }
    public uint DebugRomOffset { get; }
    public uint DebugSize { get; }
    public uint DebugRamAddress { get; }

    public NitroRomHeader(DataArchive Ar)
    {
        GameTitle = Ar.ReadString(12);
        GameCode = Ar.ReadString(4);
        MakerCode = Ar.ReadString(2);
        UnitCode = Ar.Read<NitroUnitCode>();
        EncryptionSeedSelect = Ar.Read<byte>();
        DeviceCapacity = Ar.Read<byte>();
        Ar.Position += 8;
        Region = Ar.Read<NitroRegion>();
        RomVersion = Ar.Read<byte>();
        Autostart = Ar.Read<NitroAutostart>();

        Arm9 = new ArmBinaryInfo(Ar);
        Arm7 = new ArmBinaryInfo(Ar);

        FileNameTable = new NitroTableInfo(Ar);
        FileAllocationTable = new NitroTableInfo(Ar);
        Arm9Overlay = new NitroTableInfo(Ar);
        Arm7Overlay = new NitroTableInfo(Ar);

        NormalCommandSetting = Ar.Read<uint>();
        Key1CommandSetting = Ar.Read<uint>();
        IconTitleOffset = Ar.Read<uint>();
        SecureAreaChecksum = Ar.Read<ushort>();
        SecureAreaDelay = Ar.Read<ushort>();
        Arm9AutoLoadHook = Ar.Read<uint>();
        Arm7AutoLoadHook = Ar.Read<uint>();
        SecureAreaDisable = Ar.Read<ulong>();
        TotalUsedRomSize = Ar.Read<uint>();
        HeaderSize = Ar.Read<uint>();

        Ar.Position += 0x38;

        NintendoLogo = Ar.ReadBytes(0x9C);
        LogoChecksum = Ar.Read<ushort>();
        HeaderChecksum = Ar.Read<ushort>();

        DebugRomOffset = Ar.Read<uint>();
        DebugSize = Ar.Read<uint>();
        DebugRamAddress = Ar.Read<uint>();
        Ar.Position += 4;
    }
}

public enum NitroUnitCode : byte
{
    NDS = 0b00,
    NDS_DSI = 0b10,
    DSI = 0b11
}

public enum NitroRegion : byte
{
    Normal = 0,
    Korea = 1 << 6,
    China = 1 << 7,
}

[Flags]
public enum NitroAutostart : byte
{
    None = 0,
    SkipPressButton = 1 << 2,
}

public readonly struct ArmBinaryInfo
{
    public uint RomOffset { get; }
    public uint EntryAddress { get; }
    public uint RamAddress { get; }
    public uint Size { get; }

    public ArmBinaryInfo(DataArchive Ar)
    {
        RomOffset = Ar.Read<uint>();
        EntryAddress = Ar.Read<uint>();
        RamAddress = Ar.Read<uint>();
        Size = Ar.Read<uint>();
    }
}

public readonly struct NitroTableInfo
{
    public uint Offset { get; }
    public uint Size { get; }

    public NitroTableInfo(DataArchive Ar)
    {
        Offset = Ar.Read<uint>();
        Size = Ar.Read<uint>();
    }
}
