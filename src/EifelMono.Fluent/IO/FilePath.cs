using System.IO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Text;

namespace EifelMono.Fluent.IO
{
    [DataContract]
    public class FilePath : ValuePath
    {
        #region Core things
        public FilePath() : base() { }

        public FilePath(string value) : base(value) { }

        public FilePath(string dir, string fileName) : base(Path.Combine(dir, fileName)) { }

        public FilePath(FilePath filePath) : this(filePath?.Value ?? "") { }

        public FilePath Clone()
            => new FilePath(Value);

        public static implicit operator FilePath(string path)
            => new FilePath(path);
        #endregion

        #region Actions, Properties, ...
        public string FileName
            => Path.GetFileName(Value);

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public string FullPath
            => Path.GetFullPath(Value);

        public string PathRoot
            => Path.GetPathRoot(Value);

        public string Extension
            => Path.GetExtension(Value);

        public string DirectoryName
            => Path.GetDirectoryName(Value);

        public string FileNameWithoutExtension
            => Path.GetFileNameWithoutExtension(Value);

        public bool HasExtension
            => Path.HasExtension(Value);

        public DirectoryPath Directory
            => new DirectoryPath(DirectoryName);
        #endregion

        #region Values changes

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

        public FilePath RemoveExtension()
        {
            Value = Path.Combine(DirectoryName, FileNameWithoutExtension);
            return this;
        }

        public FilePath ChangeDirectoryName(string directoryName)
        {
            Value = Path.Combine(directoryName, FileName);
            return this;
        }

        public FilePath AppendDirectoryName(string directoryName)
        {
            Value = Path.Combine(Directory.Append(directoryName), FileName);
            return this;
        }
        #endregion

        #region Actions, Copy, Move, Delete, etc...
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public bool Exists
            => File.Exists(Value);

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public bool DirectoryExists
            => Directory.Exists;

        public FilePath EnsureDirectoryExist()
        {
            Directory.EnsureExist();
            return this;
        }

        public FilePath Copy(FilePath filePath)
        {
            File.Copy(this, filePath);
            return this;
        }

        public FilePath Copy(string fileName)
        {
            File.Copy(this, new FilePath(Directory, fileName));
            return this;
        }

        public FilePath Copy(DirectoryPath directoryPath, string newFileName = null)
        {
            newFileName = newFileName ?? FileName;
            Copy(new FilePath(directoryPath, newFileName));
            return this;
        }

        public FilePath Move(FilePath filePath)
        {
            File.Move(this, filePath);
            return this;
        }

        public FilePath Move(string fileName)
        {
            File.Move(this, new FilePath(Directory, fileName));
            return this;
        }

        public FilePath Move(DirectoryPath directoryPath, string newFileName = null)
        {
            newFileName = newFileName ?? FileName;
            Move(new FilePath(directoryPath, newFileName));
            return this;
        }

        public FilePath Delete()
        {
            File.Delete(this);
            return this;
        }

        public FilePath Replace(FilePath destination, FilePath destinationBackup)
        {
            File.Replace(this, destination, destinationBackup);
            return this;
        }

        public FilePath Replace(FilePath destination, FilePath destinationBackup, bool ignoreMetadataErrors)
        {
            File.Replace(this, destination, destinationBackup, ignoreMetadataErrors);
            return this;
        }

        public FilePath Encrypt()
        {
            File.Encrypt(this);
            return this;
        }

        public FilePath Decrypt()
        {
            File.Decrypt(this);
            return this;
        }
        #endregion

        #region DateTime
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public DateTime CreationTime
        {
            get => File.GetCreationTime(Value);
            set => File.SetCreationTime(Value, value);
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public DateTime CreationTimeUtc
        {
            get => File.GetCreationTimeUtc(Value);
            set => File.SetCreationTimeUtc(Value, value);
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public DateTime LastAccessTime
        {
            get => File.GetLastAccessTime(Value);
            set => File.SetLastAccessTime(Value, value);
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public DateTime LastAccessTimeUtc
        {
            get => File.GetLastAccessTimeUtc(Value);
            set => File.SetLastAccessTimeUtc(Value, value);
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public DateTime LastWriteTime
        {
            get => File.GetLastWriteTime(Value);
            set => File.SetLastWriteTime(Value, value);
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public DateTime LastWriteTimeUtc
        {
            get => File.GetLastWriteTimeUtc(Value);
            set => File.SetLastWriteTimeUtc(Value, value);
        }
        #endregion

        #region Attributes
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public FileAttributes Attributes
            => File.GetAttributes(Value);

        public bool IsAttributes(FileAttributes fileAttributes)
            => (Attributes & fileAttributes) > 0;

        public FilePath SetAttributes(FileAttributes fileAttributes)
        {
            File.SetAttributes(Value, Attributes | fileAttributes);
            return this;
        }

        public FilePath RemoveAttributes(FileAttributes fileAttributes)
            => SetAttributes(~fileAttributes);

        public FilePath ChangeAttributes(FileAttributes fileAttributes, bool on)
        {
            if (on)
                SetAttributes(fileAttributes);
            else
                RemoveAttributes(fileAttributes);
            return this;
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public bool AttributeArchive
        {
            get => IsAttributes(FileAttributes.Archive);
            set => ChangeAttributes(FileAttributes.Archive, value);
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public bool AttributeCompressed
        {
            get => IsAttributes(FileAttributes.Compressed);
            set => ChangeAttributes(FileAttributes.Compressed, value);
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public bool AttributeDevice
        {
            get => IsAttributes(FileAttributes.Device);
            set => ChangeAttributes(FileAttributes.Device, value);
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public bool AttributeDirectory
        {
            get => IsAttributes(FileAttributes.Directory);
            set => ChangeAttributes(FileAttributes.Directory, value);
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public bool AttributeEncrypted
        {
            get => IsAttributes(FileAttributes.Encrypted);
            set => ChangeAttributes(FileAttributes.Encrypted, value);
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public bool AttributeHidden
        {
            get => IsAttributes(FileAttributes.Hidden);
            set => ChangeAttributes(FileAttributes.Hidden, value);
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public bool AttributeIntegrityStream
        {
            get => IsAttributes(FileAttributes.IntegrityStream);
            set => ChangeAttributes(FileAttributes.IntegrityStream, value);
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public bool AttributeNormal
        {
            get => IsAttributes(FileAttributes.Normal);
            set => ChangeAttributes(FileAttributes.Normal, value);
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public bool AttributeNoScrubData
        {
            get => IsAttributes(FileAttributes.NoScrubData);
            set => ChangeAttributes(FileAttributes.NoScrubData, value);
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public bool AttributeNotContentIndexed
        {
            get => IsAttributes(FileAttributes.NotContentIndexed);
            set => ChangeAttributes(FileAttributes.NotContentIndexed, value);
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public bool AttributeOffline
        {
            get => IsAttributes(FileAttributes.Offline);
            set => ChangeAttributes(FileAttributes.Offline, value);
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public bool AttributeReadOnly
        {
            get => IsAttributes(FileAttributes.ReadOnly);
            set => ChangeAttributes(FileAttributes.ReadOnly, value);
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public bool AttributeReparsePoint
        {
            get => IsAttributes(FileAttributes.ReparsePoint);
            set => ChangeAttributes(FileAttributes.ReparsePoint, value);
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public bool AttributeSparseFile
        {
            get => IsAttributes(FileAttributes.SparseFile);
            set => ChangeAttributes(FileAttributes.SparseFile, value);
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public bool AttributeSystem
        {
            get => IsAttributes(FileAttributes.System);
            set => ChangeAttributes(FileAttributes.System, value);
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public bool AttributeTemporary
        {
            get => IsAttributes(FileAttributes.Temporary);
            set => ChangeAttributes(FileAttributes.Temporary, value);
        }
        #endregion

        #region Read/Write/Append
        public byte[] ReadAllBytes()
            => File.ReadAllBytes(Value);
        public string[] ReadAllLines()
            => File.ReadAllLines(Value);
        public string[] ReadAllLines(Encoding encoding)
            => File.ReadAllLines(Value, encoding);
        public string ReadAllText()
            => File.ReadAllText(Value);
        public string ReadAllText(Encoding encoding)
            => File.ReadAllText(Value, encoding);
        public IEnumerable<string> ReadLines()
            => File.ReadLines(Value);
        public IEnumerable<string> ReadLines(Encoding encoding)
            => File.ReadLines(Value, encoding);

        public void WriteAllBytes(byte[] bytes)
            => File.WriteAllBytes(Value, bytes);
        public void WriteAllLines(IEnumerable<string> contents)
            => File.WriteAllLines(Value, contents);
        public void WriteAllLines(IEnumerable<string> contents, Encoding encoding)
            => File.WriteAllLines(Value, contents, encoding);
        public void WriteAllLines(string[] contents)
            => File.WriteAllLines(Value, contents);
        public void WriteAllLines(string[] contents, Encoding encoding)
            => File.WriteAllLines(Value, contents, encoding);
        public void WriteAllText(string contents)
            => File.WriteAllText(Value, contents);
        public void WriteAllText(string contents, Encoding encoding)
            => File.WriteAllText(Value, contents, encoding);

        public void AppendAllLines(IEnumerable<string> contents)
            => File.AppendAllLines(Value, contents);
        public void AppendAllLines(IEnumerable<string> contents, Encoding encoding)
            => File.AppendAllLines(Value, contents, encoding);
        public void AppendAllText(string contents)
            => File.AppendAllText(Value, contents);
        public void AppendAllText(string contents, Encoding encoding)
            => File.AppendAllText(Value, contents, encoding);
        #endregion

        #region Os Files
        public static class Os
        {
            public static FilePath TempFileName
                => new FilePath(Path.GetTempFileName());

            public static FilePath RandomFileName
                => new FilePath(Path.GetRandomFileName());
        }
        #endregion
    }
}
