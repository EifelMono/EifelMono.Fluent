﻿using System.IO;
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

        public FilePath Clone()
            => new FilePath(Value);

        public static implicit operator FilePath(string path)
            => new FilePath(path);

        public string FileName
            => Path.GetFileName(Value);

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
            Value = Path.Combine(Directory.Append(directoryName), FileName);
            return this;
        }
        #endregion

        #region Actions, Copy, Move, Delete, etc...
        public bool Exists
            => File.Exists(Value);

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

        #region Attributes
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

        public bool AttributeArchive
        {
            get => IsAttributes(FileAttributes.Archive);
            set => ChangeAttributes(FileAttributes.Archive, value);
        }

        public bool AttributeCompressed
        {
            get => IsAttributes(FileAttributes.Compressed);
            set => ChangeAttributes(FileAttributes.Compressed, value);
        }

        public bool AttributeDevice
        {
            get => IsAttributes(FileAttributes.Device);
            set => ChangeAttributes(FileAttributes.Device, value);
        }

        public bool AttributeDirectory
        {
            get => IsAttributes(FileAttributes.Directory);
            set => ChangeAttributes(FileAttributes.Directory, value);
        }

        public bool AttributeEncrypted
        {
            get => IsAttributes(FileAttributes.Encrypted);
            set => ChangeAttributes(FileAttributes.Encrypted, value);
        }

        public bool AttributeHidden
        {
            get => IsAttributes(FileAttributes.Hidden);
            set => ChangeAttributes(FileAttributes.Hidden, value);
        }

        public bool AttributeIntegrityStream
        {
            get => IsAttributes(FileAttributes.IntegrityStream);
            set => ChangeAttributes(FileAttributes.IntegrityStream, value);
        }

        public bool AttributeNormal
        {
            get => IsAttributes(FileAttributes.Normal);
            set => ChangeAttributes(FileAttributes.Normal, value);
        }

        public bool AttributeNoScrubData
        {
            get => IsAttributes(FileAttributes.NoScrubData);
            set => ChangeAttributes(FileAttributes.NoScrubData, value);
        }

        public bool AttributeNotContentIndexed
        {
            get => IsAttributes(FileAttributes.NotContentIndexed);
            set => ChangeAttributes(FileAttributes.NotContentIndexed, value);
        }

        public bool AttributeOffline
        {
            get => IsAttributes(FileAttributes.Offline);
            set => ChangeAttributes(FileAttributes.Offline, value);
        }

        public bool AttributeReadOnly
        {
            get => IsAttributes(FileAttributes.ReadOnly);
            set => ChangeAttributes(FileAttributes.ReadOnly, value);
        }

        public bool AttributeReparsePoint
        {
            get => IsAttributes(FileAttributes.ReparsePoint);
            set => ChangeAttributes(FileAttributes.ReparsePoint, value);
        }

        public bool AttributeSparseFile
        {
            get => IsAttributes(FileAttributes.SparseFile);
            set => ChangeAttributes(FileAttributes.SparseFile, value);
        }

        public bool AttributeSystem
        {
            get => IsAttributes(FileAttributes.System);
            set => ChangeAttributes(FileAttributes.System, value);
        }
        public bool AttributeTemporary
        {
            get => IsAttributes(FileAttributes.Temporary);
            set => ChangeAttributes(FileAttributes.Temporary, value);
        }
        #endregion

        #region Read/Write
        public string ReadAllText()
            => File.ReadAllText(Value);

        public string ReadAllLinesAsync()
        {
            File.Decrypt(Value);

            return this;
        }

        public IEnumerable<string> ReadLines()
            => File.ReadLines(Value);
        #endregion

        #region Os Files
        public FilePath TempFileName()
            => new FilePath(Path.GetTempFileName());

        public FilePath RandomFileName()
            => new FilePath(Path.GetRandomFileName());
        #endregion
    }
}
