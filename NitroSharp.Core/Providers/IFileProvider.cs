using System.Diagnostics.CodeAnalysis;
using NitroSharp.Core.Objects.Rom;

namespace NitroSharp.Core.Providers;

public interface IFileProvider
{
    IReadOnlyDictionary<string, IRomFile> Files { get; }

    bool TryGetFile(ReadOnlySpan<char> path, [NotNullWhen(true)] out IRomFile? file);
}
