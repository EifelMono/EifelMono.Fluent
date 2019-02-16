using System.IO;
using System;
using System.Collections.Generic;

namespace EifelMono.Fluent.IO
{
    public class FilePath : ValuePath
    {
        public FilePath() : base() { }

        public FilePath(string value) : base(value) { }

        public FilePath(string dir, string fileName) : base(Path.Combine(dir, fileName)) { }

        public FilePath(FilePath filePath) : this(filePath?.Value ?? "") { }

        public FilePath Clone() => new FilePath(this);

        public static implicit operator FilePath(string path)
            => new FilePath(path);

        public string FileName { get => Path.GetFileName(Value); }

        public string FullPath { get => Path.GetFullPath(Value); }

        public string PathRoot { get => Path.GetPathRoot(Value); }

        public string Extension { get => Path.GetExtension(Value); }

        public string DirectoryName { get => Path.GetDirectoryName(Value); }

        public string FileNameWithoutExtension { get => Path.GetFileNameWithoutExtension(Value); }

        public DirectoryPath Directory() => new DirectoryPath(DirectoryName);

        #region Values changes

        public FilePath MakeAbsolute()
        {
            Value = FullPath;
            return this;
        }

        public FilePath ChangeFileName(string fileName)
        {
            Value = Path.Combine(DirectoryName, fileName);
            return this;
        }

        public FilePath ChangeExtension(string extension)
        {
            Value = Path.ChangeExtension(Value, extension);
            return this;
        }

        public FilePath ChangeDirectoryName(string directoryName)
        {
            Value = Path.Combine(directoryName, FileName);
            return this;
        }

        public FilePath AppendDirectoryName(string directoryName)
        {
            Value = Path.Combine(Directory().Append(directoryName), FileName);
            return this;
        }
        #endregion

        #region Read/Write
        public string ReadAllText()
            => File.ReadAllText(this);


        public string ReadAllLinesAsync()
        {
            File.Decrypt(this);

            return this;
        }

        public IEnumerable<string> ReadLines()
            => File.ReadLines(this);
        #endregion

        #region Os Files

        public FilePath TempFileName()
            => new FilePath(Path.GetTempFileName());

        public FilePath RandomFileName()
            => new FilePath(Path.GetRandomFileName());

        #endregion

    }
}
