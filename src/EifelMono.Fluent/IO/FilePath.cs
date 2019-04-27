#nullable enable
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using EifelMono.Fluent.Classes;
using EifelMono.Fluent.Extensions;

namespace EifelMono.Fluent.IO
{
    [DataContract]
    public partial class FilePath : ValuePath, IFluentExists
    {
        #region Core things
        public FilePath() : base() { }

        public FilePath(string fileName) : base(fileName) { }

        public FilePath(string directoryName, string fileName) : base(Path.Combine(directoryName, fileName)) { }

        public FilePath Clone(string newFileName = "")
            => newFileName.IsNullOrEmpty() ? new FilePath(Value) : new FilePath(DirectoryName, newFileName);

        public static implicit operator FilePath(string path)
            => new FilePath(path);
        #endregion

        #region Properties, ...
        public string FileName
            => Path.GetFileName(Value);

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

        public FilePath ChangeFileNameWithoutExtension(string fileNameWithoutExtension)
        {
            Value = Path.Combine(DirectoryName, $"{fileNameWithoutExtension}{Extension}");
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

        public FilePath SetCurrentDirectory()
        {
            Directory.SetCurrentDirectory();
            return this;
        }

        public FilePath Copy(FilePath filePath, bool overwrite = true)
        {
            File.Copy(this, filePath, overwrite);
            return this;
        }

        public FilePath Copy(string fileName, bool overwrite = true)
        {
            File.Copy(this, new FilePath(Directory, fileName), overwrite);
            return this;
        }

        public FilePath Copy(DirectoryPath directoryPath, string? newFileName = null, bool overwrite = true)
        {
            if (newFileName == null)
                newFileName = FileName;
            Copy(new FilePath(directoryPath, newFileName), overwrite);
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

        public FilePath Move(DirectoryPath directoryPath, string? newFileName = null)
        {
            if (newFileName == null)
                newFileName = FileName;
            Move(new FilePath(directoryPath, newFileName));
            return this;
        }

        public FilePath Delete()
        {
            File.Delete(this);
            return this;
        }


#if !NETSTANDARD1_6
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
#endif
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

        public FilePath ClearAttributes()
        {
            File.SetAttributes(Value, 0);
            return this;
        }

        public FilePath SetAttributes(FileAttributes fileAttributes)
        {
            File.SetAttributes(Value, Attributes | fileAttributes);
            return this;
        }

        public FilePath RemoveAttributes(FileAttributes fileAttributes)
        {
            File.SetAttributes(Value, Attributes & ~fileAttributes);
            return this;
        }

        public FilePath RemoveAttributes()
        {
            fluent.Enum.Values<FileAttributes>().ForEach((a) => RemoveAttributes(a));
            return this;
        }

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
        public FilePath WriteAllBytes(byte[] bytes)
        {
            File.WriteAllBytes(Value, bytes);
            return this;
        }
        public FilePath WriteAllLines(IEnumerable<string> contents)
        {
            File.WriteAllLines(Value, contents);
            return this;
        }
        public FilePath WriteAllLines(IEnumerable<string> contents, Encoding encoding)
        {
            File.WriteAllLines(Value, contents, encoding);
            return this;
        }
        public FilePath WriteAllLines(string[] contents)
        {
            File.WriteAllLines(Value, contents);
            return this;
        }
        public FilePath WriteAllLines(string[] contents, Encoding encoding)
        {
            File.WriteAllLines(Value, contents, encoding);
            return this;
        }
        public FilePath WriteAllText(string contents)
        {
            File.WriteAllText(Value, contents);
            return this;
        }
        public FilePath WriteAllText(string contents, Encoding encoding)
        {
            File.WriteAllText(Value, contents, encoding);
            return this;
        }
        public FilePath AppendAllLines(IEnumerable<string> contents)
        {
            File.AppendAllLines(Value, contents);
            return this;
        }
        public FilePath AppendAllLines(IEnumerable<string> contents, Encoding encoding)
        {
            File.AppendAllLines(Value, contents, encoding);
            return this;
        }
        public FilePath AppendAllText(string contents)
        {
            File.AppendAllText(Value, contents);
            return this;
        }
        public FilePath AppendAllText(string contents, Encoding encoding)
        {
            File.AppendAllText(Value, contents, encoding);
            return this;
        }

        public FilePath Write(string content)
        {
            AppendAllText(content);
            return this;
        }
        public FilePath WriteLine(string content)
        {
            Write($"{content}{Environment.NewLine}");
            return this;
        }
        #endregion

        #region OS Files
        public static class OS
        {
            public static FilePath Current
             => new FilePath(Environment.GetCommandLineArgs()[0]);
            public static FilePath Temp
                => new FilePath(Path.GetTempFileName());
            public static FilePath Random
                => new FilePath(Path.GetRandomFileName());
        }
        #endregion
    }
}
