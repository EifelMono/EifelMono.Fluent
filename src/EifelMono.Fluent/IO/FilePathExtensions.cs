using System.IO;
using System.Collections.Generic;

namespace EifelMono.Fluent.IO
{
    public static class FilePathExtensions
    {

        public static DirectoryPath Directory(this FilePath thisValue)
            => new DirectoryPath(thisValue.DirectoryName);

        public static FilePath AppendDirectory(this FilePath thisValue, params string[] directories)
            =>  new FilePath(thisValue.Directory().Combine(directories), thisValue.FileName);

        public static FilePath ChangeDirectory(this FilePath thisValue, string directory)
           => new FilePath(directory, thisValue.FileName);

        public static string ReadAllText(this FilePath thisValue)
            => File.ReadAllText(thisValue);

        public static IEnumerable<string> ReadLines(this FilePath thisValue)
            => File.ReadLines(thisValue);
    }
}
