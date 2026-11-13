using NitroSharp.Core.Data;

namespace NitroSharp.Core.Providers;

public static class FileProviderExtensions
{
    extension(IFileProvider provider)
    {
        public T? LoadObject<T>(string path) where T : class, INitroDeserializable<T>
            => provider.LoadFile(path)?.Load<T>();

        public T? LoadObject<T>(ushort id) where T : class, INitroDeserializable<T>
            => provider.LoadFile(id)?.Load<T>();
    }
}
