using System.IO;
using System;

namespace EifelMono.Fluent.IO
{
    public class FilePath : ValuePath
    {
        public FilePath() : base() { }

        public FilePath(string value) : base(value) { }

        public FilePath(string dir, string fileName) : base(Path.Combine(dir, fileName)) { }

        public FilePath(FilePath filePath) : this(filePath?.Value ?? "") { }

        public static implicit operator FilePath(string path)
            => new FilePath(path);

        public string FileName { get => Path.GetFileName(Value); }

        public string FullPath { get => Path.GetFullPath(Value); }

        public string PathRoot { get => Path.GetPathRoot(Value); }

        public string Extension { get => Path.GetExtension(Value); }

        public string DirectoryName { get => Path.GetDirectoryName(Value); }

        public string FileNameWithoutExtension { get => Path.GetFileNameWithoutExtension(Value); }


        #region Os Directories

        public FilePath TempFileName()
            => new FilePath(Path.GetTempFileName());

        public FilePath RandomFileName()
            => new FilePath(Path.GetRandomFileName());

        #endregion

    }
}
