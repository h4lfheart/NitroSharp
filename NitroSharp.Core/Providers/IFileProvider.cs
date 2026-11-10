using System.Diagnostics.CodeAnalysis;
using NitroSharp.Core.Data.Archive;
using NitroSharp.Core.Objects.Files;
using NitroSharp.Core.Objects.Rom;

namespace NitroSharp.Core.Providers;

public interface IFileProvider
{
    IReadOnlyDictionary<string, IRomFile> Files { get; }

    IRomFile? LoadFile(string path);

    IRomFile? LoadFile(ushort id);

    DataArchive Open(IRomFile file);

}
