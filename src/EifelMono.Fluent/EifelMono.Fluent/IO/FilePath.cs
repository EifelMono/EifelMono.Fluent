using System.IO;
using System;

namespace EifelMono.Fluent.IO
{
    public class FilePath : ValuePath
    {
        public FilePath() : base() { }

        public FilePath(string value) : base(value) { }

        public FilePath(FilePath filePath) : this(filePath?.Value ?? "") { }

        public string FileName { get => Path.GetFileName(Value);}

        public string FullPath { get => Path.GetFullPath(Value); }

        public string PathRoot { get => Path.GetPathRoot(Value); }

        public string Extension { get => Path.GetExtension(Value); }

        public string DirectoryName { get => Path.GetDirectoryName(Value); }

        public string FileNameWithoutExtension { get => Path.GetFileNameWithoutExtension(Value); }


       

    }
}
