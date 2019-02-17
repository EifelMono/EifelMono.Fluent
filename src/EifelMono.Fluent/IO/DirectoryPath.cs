using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace EifelMono.Fluent.IO
{
    public class DirectoryPath : ValuePath
    {
        #region Core things
        public DirectoryPath() : base() { }

        public DirectoryPath(string value) : base(value) { }

        public DirectoryPath(DirectoryPath directoryPath) : this(directoryPath?.Value ?? "") { }

        public DirectoryPath Clone()
            => new DirectoryPath(Value);

        public static implicit operator DirectoryPath(string path)
            => new DirectoryPath(path);
        #endregion

        #region Actions, ...

        public FilePath MakeFilePath(string fileName)
            => new FilePath(Value, fileName);

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public string FullPath
            => Path.GetFullPath(Value);

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public bool Exists
            => Directory.Exists(Value);

        #endregion

        #region Values changes

        public DirectoryPath MakeAbsolute()
        {
            Value = FullPath;
            return this;
        }

        public DirectoryPath Normalize()
        {
            Value = Value.NormalizePath();
            return this;
        }

        public DirectoryPath Append(params string[] paths)
        {
            Value = Path.Combine(new string[] { Value }.Concat(paths).ToArray());
            return this;
        }

        #endregion

        public DirectoryPath EnsureExist(FluentExAction<DirectoryPath> fluentExAction = default)
        {
            try
            {
                if (!Directory.Exists(Value))
                    Directory.CreateDirectory(Value);
            }
            catch (Exception ex)
            {
                if (fluentExAction?.Invoke(ex, Value) is var result && result != null && result.Fixed)
                    return this;
                throw ex;
            }
            return this;
        }

        private void CleanAndOrDelete(DirectoryInfo baseDirectory, bool recursive, bool deleteDir)
        {
            if (!baseDirectory.Exists)
                return;

            if (recursive)
                foreach (var directory in baseDirectory.EnumerateDirectories())
                    CleanAndOrDelete(directory, recursive, deleteDir);

            foreach (var file in baseDirectory.GetFiles())
            {
                file.IsReadOnly = false;
                file.Delete();
            }

            if (deleteDir)
                baseDirectory.Delete();
        }

        public DirectoryPath Delete(string searchPattern = "*")
        {
            // TODO
            CleanAndOrDelete(new DirectoryInfo(Value), false, true);
            return this;
        }

        public DirectoryPath Clean(string searchPattern = "*")
        {
            // TODO
            CleanAndOrDelete(new DirectoryInfo(Value), false, false);
            return this;
        }

        public IEnumerable<FilePath> GetFiles(string searchPattern = "*")
        {
            throw new NotImplementedException();
        }

        #region Os Directories

        public DirectoryPath Current()
            => new DirectoryPath(Directory.GetCurrentDirectory());

        public DirectoryPath Temp()
            => new DirectoryPath(Path.GetTempPath());

        public static DirectoryPath SpezialDirectoryPath(Environment.SpecialFolder specialFolder)
            => new DirectoryPath(Environment.GetFolderPath(specialFolder));

        public static class SpezialDirectory
        {
            public static DirectoryPath AdminTools()
                => SpezialDirectoryPath(Environment.SpecialFolder.AdminTools);
            public static DirectoryPath ApplicationData()
                => SpezialDirectoryPath(Environment.SpecialFolder.ApplicationData);
            public static DirectoryPath CDBurning()
                 => SpezialDirectoryPath(Environment.SpecialFolder.CDBurning);
            public static DirectoryPath CommonAdminTools()
                => SpezialDirectoryPath(Environment.SpecialFolder.CommonAdminTools);
            public static DirectoryPath CommonApplicationData()
                => SpezialDirectoryPath(Environment.SpecialFolder.CommonApplicationData);
            public static DirectoryPath CommonDesktopDirectory()
                 => SpezialDirectoryPath(Environment.SpecialFolder.CommonDesktopDirectory);
            public static DirectoryPath CommonDocuments()
                => SpezialDirectoryPath(Environment.SpecialFolder.CommonDocuments);
            public static DirectoryPath CommonMusic()
                => SpezialDirectoryPath(Environment.SpecialFolder.CommonMusic);
            public static DirectoryPath CommonOemLinks()
                 => SpezialDirectoryPath(Environment.SpecialFolder.CommonOemLinks);
            public static DirectoryPath CommonPictures()
                => SpezialDirectoryPath(Environment.SpecialFolder.CommonPictures);
            public static DirectoryPath CommonProgramFiles()
                => SpezialDirectoryPath(Environment.SpecialFolder.CommonProgramFiles);
            public static DirectoryPath CommonProgramFilesX86()
                 => SpezialDirectoryPath(Environment.SpecialFolder.CommonProgramFilesX86);
            public static DirectoryPath CommonPrograms()
                => SpezialDirectoryPath(Environment.SpecialFolder.CommonPrograms);
            public static DirectoryPath CommonStartMenu()
                => SpezialDirectoryPath(Environment.SpecialFolder.CommonStartMenu);
            public static DirectoryPath CommonStartup()
                 => SpezialDirectoryPath(Environment.SpecialFolder.CommonStartup);
            public static DirectoryPath CommonTemplates()
                => SpezialDirectoryPath(Environment.SpecialFolder.CommonTemplates);
            public static DirectoryPath CommonVideos()
                => SpezialDirectoryPath(Environment.SpecialFolder.CommonVideos);
            public static DirectoryPath Cookies()
                 => SpezialDirectoryPath(Environment.SpecialFolder.Cookies);
            public static DirectoryPath Desktop()
                => SpezialDirectoryPath(Environment.SpecialFolder.Desktop);
            public static DirectoryPath DesktopDirectory()
                => SpezialDirectoryPath(Environment.SpecialFolder.DesktopDirectory);
            public static DirectoryPath Favorites()
                 => SpezialDirectoryPath(Environment.SpecialFolder.Favorites);
            public static DirectoryPath Fonts()
                => SpezialDirectoryPath(Environment.SpecialFolder.Fonts);
            public static DirectoryPath History()
                => SpezialDirectoryPath(Environment.SpecialFolder.History);
            public static DirectoryPath InternetCache()
                 => SpezialDirectoryPath(Environment.SpecialFolder.InternetCache);
            public static DirectoryPath LocalApplicationData()
                => SpezialDirectoryPath(Environment.SpecialFolder.LocalApplicationData);
            public static DirectoryPath LocalizedResources()
                => SpezialDirectoryPath(Environment.SpecialFolder.LocalizedResources);
            public static DirectoryPath MyComputer()
                 => SpezialDirectoryPath(Environment.SpecialFolder.MyComputer);
            public static DirectoryPath MyDocuments()
                => SpezialDirectoryPath(Environment.SpecialFolder.MyDocuments);
            public static DirectoryPath MyMusic()
                => SpezialDirectoryPath(Environment.SpecialFolder.MyMusic);
            public static DirectoryPath MyPictures()
                 => SpezialDirectoryPath(Environment.SpecialFolder.MyPictures);
            public static DirectoryPath MyVideos()
                => SpezialDirectoryPath(Environment.SpecialFolder.MyVideos);
            public static DirectoryPath NetworkShortcuts()
                => SpezialDirectoryPath(Environment.SpecialFolder.NetworkShortcuts);
            public static DirectoryPath Personal()
                 => SpezialDirectoryPath(Environment.SpecialFolder.Personal);
            public static DirectoryPath PrinterShortcuts()
                => SpezialDirectoryPath(Environment.SpecialFolder.PrinterShortcuts);
            public static DirectoryPath ProgramFiles()
                => SpezialDirectoryPath(Environment.SpecialFolder.ProgramFiles);
            public static DirectoryPath ProgramFilesX86()
                 => SpezialDirectoryPath(Environment.SpecialFolder.ProgramFilesX86);
            public static DirectoryPath Programs()
                => SpezialDirectoryPath(Environment.SpecialFolder.Programs);
            public static DirectoryPath Recent()
                => SpezialDirectoryPath(Environment.SpecialFolder.Recent);
            public static DirectoryPath Resources()
                 => SpezialDirectoryPath(Environment.SpecialFolder.Resources);
            public static DirectoryPath SendTo()
                => SpezialDirectoryPath(Environment.SpecialFolder.SendTo);
            public static DirectoryPath StartMenu()
                => SpezialDirectoryPath(Environment.SpecialFolder.StartMenu);
            public static DirectoryPath Startup()
                 => SpezialDirectoryPath(Environment.SpecialFolder.Startup);
            public static DirectoryPath System()
                => SpezialDirectoryPath(Environment.SpecialFolder.System);
            public static DirectoryPath SystemX86()
                => SpezialDirectoryPath(Environment.SpecialFolder.SystemX86);
            public static DirectoryPath Templates()
                 => SpezialDirectoryPath(Environment.SpecialFolder.Templates);
            public static DirectoryPath FUserProfileonts()
                => SpezialDirectoryPath(Environment.SpecialFolder.UserProfile);
            public static DirectoryPath Windows()
                => SpezialDirectoryPath(Environment.SpecialFolder.Windows);
        }

        #endregion
    }
}
