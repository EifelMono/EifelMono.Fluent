using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using EifelMono.Fluent.Extensions;

namespace EifelMono.Fluent.IO
{
    [DataContract]
    public class DirectoryPath : ValuePath
    {
        #region Core things
        public DirectoryPath() : base() { }

        public DirectoryPath(string directoryName) : base(directoryName) { }

        public DirectoryPath Clone(params string[] appendPaths)
            => new DirectoryPath(Value).Append(appendPaths);
        public FilePath CloneToFilePath(string fileName)
            => new FilePath(Value, fileName);

        public static implicit operator DirectoryPath(string path)
            => new DirectoryPath(path);
        #endregion

        #region Actions, ...
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public bool Exists
        {
            get
            {
                try
                {
                    return Directory.Exists(Value);
                }
                catch
                {
                    return false;
                }
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public string DirectoryRoot
            => Directory.GetDirectoryRoot(Value);

#if !NETSTANDARD1_6
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public List<string> LogicalDrives
            => Directory.GetLogicalDrives().ToList();
#endif
        public DirectoryPath Parent
            => new DirectoryPath(Directory.GetParent(Value).FullName);

        public DirectoryPath Move(DirectoryPath destinationDirectory)
        {
            Directory.Move(Value, destinationDirectory);
            return this;
        }
        #endregion

        #region Values changes

        public DirectoryPath Append(params string[] paths)
        {
            if (paths.Length > 0)
                Value = Path.Combine(new string[] { Value }.Concat(paths).ToArray());
            return this;
        }

        #endregion

        #region DateTime
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public DateTime CreationTime
        {
            get => Directory.GetCreationTime(Value);
            set => Directory.SetCreationTime(Value, value);
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public DateTime CreationTimeUtc
        {
            get => Directory.GetCreationTimeUtc(Value);
            set => Directory.SetCreationTimeUtc(Value, value);
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public DateTime LastAccessTime
        {
            get => Directory.GetLastAccessTime(Value);
            set => Directory.SetLastAccessTime(Value, value);
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public DateTime LastAccessTimeUtc
        {
            get => Directory.GetLastAccessTimeUtc(Value);
            set => Directory.SetLastAccessTimeUtc(Value, value);
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public DateTime LastWriteTime
        {
            get => Directory.GetLastWriteTime(Value);
            set => Directory.SetLastWriteTime(Value, value);
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public DateTime LastWriteTimeUtc
        {
            get => Directory.GetLastWriteTimeUtc(Value);
            set => Directory.SetLastWriteTimeUtc(Value, value);
        }
        #endregion

        #region Others
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public DirectoryInfo DirectoryInfo => new DirectoryInfo(Value);
        #endregion

        #region Actions

        public DirectoryPath EnsureExist()
        {
            if (!Directory.Exists(Value))
                Directory.CreateDirectory(Value);
            return this;
        }

        public DirectoryPath IfNotExist(Action<DirectoryPath> action)
        {
            if (!Exists)
                action?.Invoke(this);
            return this;
        }

        public DirectoryPath IfExist(Action<DirectoryPath> action)
        {
            if (Exists)
                action?.Invoke(this);
            return this;
        }

        public async Task<DirectoryPath> DeleteAsync(string searchMask = "**")
        {
            foreach (var directory in (await GetDirectoriesAsync(searchMask).ConfigureAwait(false)).Pipe(d => d.Reverse()))
                Directory.Delete(directory);
            return this;
        }

        public DirectoryPath Delete(string searchMask = "**")
            => Task.Run(async () => await DeleteAsync(searchMask).ConfigureAwait(false)).Result;

        public async Task<DirectoryPath> CleanAsync(string searchMask = "**\\*")
        {
            foreach (var filePath in await GetFilesAsync(searchMask).ConfigureAwait(false))
                filePath.Delete();
            return this;
        }

        public DirectoryPath Clean(string searchMask = "**\\*")
         => Task.Run(async () => await CleanAsync(searchMask).ConfigureAwait(false)).Result;

        public async Task<List<DirectoryPath>> GetDirectoriesAsync(string searchMask)
            => await new MaskPath(searchMask).GetDirectoriesAsync(Value).ConfigureAwait(false);

        public List<DirectoryPath> GetDirectories(string searchMask)
            => Task.Run(async () => await GetDirectoriesAsync(searchMask).ConfigureAwait(false)).Result;

        public async Task<List<FilePath>> GetFilesAsync(string searchMask)
            => await new MaskPath(searchMask).GetFilesAsync(Value).ConfigureAwait(false);

        public List<FilePath> GetFiles(string searchMask)
            => Task.Run(async () => await GetFilesAsync(searchMask).ConfigureAwait(false)).Result;

        public async Task<DirectoryPath> DeleteFilesAsync(string searchMask)
        {
            foreach (var filePath in await GetFilesAsync(searchMask).ConfigureAwait(false))
                filePath.Delete();
            return this;
        }
        public DirectoryPath DeleteFiles(string searchMask)
            => Task.Run(async () => await DeleteFilesAsync(searchMask).ConfigureAwait(false)).Result;

        #endregion

        #region OS Directories

        public static class OS
        {
            public static DirectoryPath Current
                => new DirectoryPath(Directory.GetCurrentDirectory());

            public static DirectoryPath Temp
                => new DirectoryPath(Path.GetTempPath());

#if ! NETSTANDARD1_6
            public static DirectoryPath Data
                => SpezialDirectory.CommonApplicationData;

#pragma warning disable IDE1006 // Naming Styles
            public static DirectoryPath dotnet
#pragma warning restore IDE1006 // Naming Styles
                => SpezialDirectory.UserProfile.Clone(".dotnet");

#pragma warning disable IDE1006 // Naming Styles
            public static DirectoryPath dotnettools
#pragma warning restore IDE1006 // Naming Styles
                => SpezialDirectory.UserProfile.Clone(".dotnet", "tools");

#pragma warning disable IDE1006 // Naming Styles
            public static DirectoryPath nuget
#pragma warning restore IDE1006 // Naming Styles
                => SpezialDirectory.UserProfile.Clone(".nuget");

            public static DirectoryPath SpecialFolderPath(Environment.SpecialFolder specialFolder)
                => new DirectoryPath(Environment.GetFolderPath(specialFolder));

            public static class SpezialDirectory
            {
                public static DirectoryPath AdminTools
                    => SpecialFolderPath(Environment.SpecialFolder.AdminTools);
                public static DirectoryPath ApplicationData
                    => SpecialFolderPath(Environment.SpecialFolder.ApplicationData);
                public static DirectoryPath CDBurning
                     => SpecialFolderPath(Environment.SpecialFolder.CDBurning);
                public static DirectoryPath CommonAdminTools
                    => SpecialFolderPath(Environment.SpecialFolder.CommonAdminTools);
                public static DirectoryPath CommonApplicationData
                    => SpecialFolderPath(Environment.SpecialFolder.CommonApplicationData);
                public static DirectoryPath CommonDesktopDirectory
                     => SpecialFolderPath(Environment.SpecialFolder.CommonDesktopDirectory);
                public static DirectoryPath CommonDocuments
                    => SpecialFolderPath(Environment.SpecialFolder.CommonDocuments);
                public static DirectoryPath CommonMusic
                    => SpecialFolderPath(Environment.SpecialFolder.CommonMusic);
                public static DirectoryPath CommonOemLinks
                     => SpecialFolderPath(Environment.SpecialFolder.CommonOemLinks);
                public static DirectoryPath CommonPictures
                    => SpecialFolderPath(Environment.SpecialFolder.CommonPictures);
                public static DirectoryPath CommonProgramFiles
                    => SpecialFolderPath(Environment.SpecialFolder.CommonProgramFiles);
                public static DirectoryPath CommonProgramFilesX86
                     => SpecialFolderPath(Environment.SpecialFolder.CommonProgramFilesX86);
                public static DirectoryPath CommonPrograms
                    => SpecialFolderPath(Environment.SpecialFolder.CommonPrograms);
                public static DirectoryPath CommonStartMenu
                    => SpecialFolderPath(Environment.SpecialFolder.CommonStartMenu);
                public static DirectoryPath CommonStartup
                     => SpecialFolderPath(Environment.SpecialFolder.CommonStartup);
                public static DirectoryPath CommonTemplates
                    => SpecialFolderPath(Environment.SpecialFolder.CommonTemplates);
                public static DirectoryPath CommonVideos
                    => SpecialFolderPath(Environment.SpecialFolder.CommonVideos);
                public static DirectoryPath Cookies
                     => SpecialFolderPath(Environment.SpecialFolder.Cookies);
                public static DirectoryPath Desktop
                    => SpecialFolderPath(Environment.SpecialFolder.Desktop);
                public static DirectoryPath DesktopDirectory
                    => SpecialFolderPath(Environment.SpecialFolder.DesktopDirectory);
                public static DirectoryPath Favorites
                     => SpecialFolderPath(Environment.SpecialFolder.Favorites);
                public static DirectoryPath Fonts
                    => SpecialFolderPath(Environment.SpecialFolder.Fonts);
                public static DirectoryPath History
                    => SpecialFolderPath(Environment.SpecialFolder.History);
                public static DirectoryPath InternetCache
                     => SpecialFolderPath(Environment.SpecialFolder.InternetCache);
                public static DirectoryPath LocalApplicationData
                    => SpecialFolderPath(Environment.SpecialFolder.LocalApplicationData);
                public static DirectoryPath LocalizedResources
                    => SpecialFolderPath(Environment.SpecialFolder.LocalizedResources);
                public static DirectoryPath MyComputer
                     => SpecialFolderPath(Environment.SpecialFolder.MyComputer);
                public static DirectoryPath MyDocuments
                    => SpecialFolderPath(Environment.SpecialFolder.MyDocuments);
                public static DirectoryPath MyMusic
                    => SpecialFolderPath(Environment.SpecialFolder.MyMusic);
                public static DirectoryPath MyPictures
                     => SpecialFolderPath(Environment.SpecialFolder.MyPictures);
                public static DirectoryPath MyVideos
                    => SpecialFolderPath(Environment.SpecialFolder.MyVideos);
                public static DirectoryPath NetworkShortcuts
                    => SpecialFolderPath(Environment.SpecialFolder.NetworkShortcuts);
                public static DirectoryPath Personal
                     => SpecialFolderPath(Environment.SpecialFolder.Personal);
                public static DirectoryPath PrinterShortcuts
                    => SpecialFolderPath(Environment.SpecialFolder.PrinterShortcuts);
                public static DirectoryPath ProgramFiles
                    => SpecialFolderPath(Environment.SpecialFolder.ProgramFiles);
                public static DirectoryPath ProgramFilesX86
                     => SpecialFolderPath(Environment.SpecialFolder.ProgramFilesX86);
                public static DirectoryPath Programs
                    => SpecialFolderPath(Environment.SpecialFolder.Programs);
                public static DirectoryPath Recent
                    => SpecialFolderPath(Environment.SpecialFolder.Recent);
                public static DirectoryPath Resources
                     => SpecialFolderPath(Environment.SpecialFolder.Resources);
                public static DirectoryPath SendTo
                    => SpecialFolderPath(Environment.SpecialFolder.SendTo);
                public static DirectoryPath StartMenu
                    => SpecialFolderPath(Environment.SpecialFolder.StartMenu);
                public static DirectoryPath Startup
                     => SpecialFolderPath(Environment.SpecialFolder.Startup);
                public static DirectoryPath System
                    => SpecialFolderPath(Environment.SpecialFolder.System);
                public static DirectoryPath SystemX86
                    => SpecialFolderPath(Environment.SpecialFolder.SystemX86);
                public static DirectoryPath Templates
                     => SpecialFolderPath(Environment.SpecialFolder.Templates);
                public static DirectoryPath UserProfile
                    => SpecialFolderPath(Environment.SpecialFolder.UserProfile);
                public static DirectoryPath Windows
                    => SpecialFolderPath(Environment.SpecialFolder.Windows);
            }
#endif
        }
        #endregion
    }
}
