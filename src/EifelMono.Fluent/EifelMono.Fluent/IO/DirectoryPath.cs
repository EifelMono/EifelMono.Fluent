using System;
using System.IO;

namespace EifelMono.Fluent.IO
{
    public class DirectoryPath : ValuePath
    {
        public DirectoryPath() : base() { }

        public DirectoryPath(string value) : base(value) { }

        public DirectoryPath(DirectoryPath directoryPath) : this(directoryPath?.Value ?? "") { }

        #region Operation
        public static implicit operator DirectoryPath(string path)
            => new DirectoryPath(path);


        #endregion

        public bool Exist
            => Directory.Exists(Value);

        #region static Environment.GetFolderPath(Environment.SpecialFolder specialFolder)

        public static DirectoryPath FolderPath(Environment.SpecialFolder specialFolder)
            => new DirectoryPath(Environment.GetFolderPath(specialFolder));


        #endregion
    }
}
