using NitroSharp.Core.Objects.Exports.Screens;
using NitroSharp.Conversion.Screens;

namespace NitroSharp.Conversion.Screens;

public static class ScreenConverter
{
    public static CScreen Decode(this NitroNSCR nscr)
    {
        var block = nscr.ScreenData;
        return new CScreen(block.Width, block.Height, block.Entries);
    }
}
