<div align="center">

# NitroSharp

A Nintendo DS (.nds) parsing and extraction library for .NET


</div>

## Requirements

- [.NET 10 SDK](https://dotnet.microsoft.com/download) or newer

## Quick Start

Add `NitroSharp.Core` and `NitroSharp.Conversion` to your project, then:

```cs
using NitroSharp.Core.Objects.Exports.Archive;
using NitroSharp.Core.Objects.Exports.Graphics;
using NitroSharp.Core.Objects.Exports.Palettes;
using NitroSharp.Core.Objects.Exports.Screens;
using NitroSharp.Core.Providers;

var provider = new NitroFileProvider("game.nds");
provider.Initialize();

// load an archive file
var narc = provider.LoadObject<NitroNARC>("a/0/4/0")!;

// load the relevant objects and decode
var screen = narc.LoadObject<NitroNSCR>(32).Decode();
var graphic = narc.LoadObject<NitroNCGR>(19).Decode();
var palette = narc.LoadObject<NitroNCLR>(4).Decode();

var image = screen.ToImage(graphic, palette);
```

## Building from Source

```sh
dotnet build NitroSharp.slnx -c Release
```
