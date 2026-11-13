<div align="center">

# NitroSharp

A Nintendo DS (.nds) parsing and extraction library for .NET


</div>

## Requirements

- [.NET 10 SDK](https://dotnet.microsoft.com/download) or newer

## Quick Start

Add `NitroSharp.Core` and `NitroSharp.Conversion` to your project, then:

```cs
// Create the file interface to read game files
var provider = new NitroFileProvider("game.nds");
provider.Initialize();

// Load an archive file to browse the inner assets
var narc = provider.LoadObject<NitroNARC>("a/0/4/0")!;

// Load the relevant objects and decode them to their conversion classes
var screen = narc.LoadObject<NitroNSCR>(32).Decode();
var graphic = narc.LoadObject<NitroNCGR>(19).Decode();
var palette = narc.LoadObject<NitroNCLR>(4).Decode();

// Create the final image from the screen indices, graphic data, and palette
var image = screen.ToImage(graphic, palette);
```

## Building from Source

```sh
dotnet build NitroSharp.slnx -c Release
```
