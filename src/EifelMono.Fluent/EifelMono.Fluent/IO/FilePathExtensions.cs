﻿using System.IO;
using System.Collections.Generic;

namespace EifelMono.Fluent.IO
{
    public static class FilePathExtensions
    {

        public static DirectoryPath Directory(this FilePath thisValue)
            => new DirectoryPath(thisValue.DirectoryName);

        public static FilePath AppendDirectory(this FilePath thisValue, params string[] directories)
        {
            thisValue.Value = Path.Combine(thisValue.Directory().Combine(directories).Value, thisValue.FileName);
            return thisValue;
        }

        public static FilePath ChangeDirectory(this FilePath thisValue, params string[] directories)
        {
            thisValue.Value = Path.Combine(thisValue.Directory().Combine(directories).Value, thisValue.FileName);
            return thisValue;
        }

        public static string ReadAllText(this FilePath thisValue)
            => File.ReadAllText(thisValue.Value);

        public static IEnumerable<string> ReadLines(this FilePath thisValue)
            => File.ReadLines(thisValue.Value);
    }
}
