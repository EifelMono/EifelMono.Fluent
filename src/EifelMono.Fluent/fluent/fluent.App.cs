using EifelMono.Fluent.IO;

namespace EifelMono.Fluent
{
#pragma warning disable IDE1006 // Naming Styles
    public static partial class fluent
#pragma warning restore IDE1006 // Naming Styles
    {
        public static class App
        {
            public static FilePath Executable
                => FilePath.OS.Current;
        }
    }
}
