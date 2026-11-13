using NitroSharp.Core.Objects.Colors;
using NitroSharp.Core.Objects.Exports.Graphics;

namespace NitroSharp.Conversion.Images;

public static class ImageDecoder
{
    private const int TileSize = 8;
    private const int TilePixels = TileSize * TileSize;

    public static CIndexedImage DecodeIndices(NitroGraphicDataBlock graphicData)
    {
        var indices = graphicData.ColorDepth switch
        {
            NitroColorDepth.Bpp4 => DecodeIndices4Bpp(graphicData.TileData),
            NitroColorDepth.Bpp8 => graphicData.TileData,
            _ => throw new NotSupportedException($"Unsupported color depth {graphicData.ColorDepth}."),
        };

        var tileWidth = graphicData.TileWidth;
        var tileHeight = graphicData.TileHeight;
        if (tileWidth is 0xFFFF or 0 || tileHeight is 0xFFFF or 0)
        {
            tileWidth = 4;
            tileHeight = (ushort)(indices.Length / (TileSize * TileSize * tileWidth));
        }

        var width = tileWidth * TileSize;
        var height = tileHeight * TileSize;

        if ((graphicData.Flags & 1) == 0)
            indices = Unswizzle(indices, width);

        return new CIndexedImage(width, height, indices, graphicData.ColorDepth);
    }

    private static byte[] DecodeIndices4Bpp(byte[] data)
    {
        var indices = new byte[data.Length * 2];
        for (var i = 0; i < indices.Length; i++)
            indices[i] = (byte)((data[i / 2] >> ((i % 2) * 4)) & 0b1111);

        return indices;
    }

    private static byte[] Unswizzle(byte[] indices, int width)
    {
        var output = new byte[indices.Length];
        for (var i = 0; i < indices.Length; i++)
        {
            var x = i % width;
            var y = i / width;
            var tiledIndex = GetTiledIndex(x, y, width);
            output[i] = tiledIndex < indices.Length ? indices[tiledIndex] : indices[i];
        }

        return output;
    }

    private static int GetTiledIndex(int x, int y, int width)
    {
        var horizontalTiles = width / TileSize;
        return (y / TileSize) * horizontalTiles * TilePixels
             + (x / TileSize) * TilePixels
             + (y % TileSize) * TileSize
             + x % TileSize;
    }
}
