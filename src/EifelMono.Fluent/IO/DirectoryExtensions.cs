using System;
using System.IO;
using System.Linq;

namespace EifelMono.Fluent.IO
{
    public static class DirectoryExtensions
    {

        public static DirectoryPath Combine(this DirectoryPath thisValue, params string[] directories)
            => new DirectoryPath(Path.Combine((new string[] { thisValue }).Concat(directories).ToArray()));

        public static DirectoryPath FullPath(this DirectoryPath thisValue)
           => new DirectoryPath(thisValue.FullPath);

        public static DirectoryPath EnsureExist(this DirectoryPath thisValue, FluentExAction<DirectoryPath> fluentExAction = default)
        {
            try
            {
                if (!Directory.Exists(thisValue))
                    Directory.CreateDirectory(thisValue);
            }
            catch (Exception ex)
            {
                if (fluentExAction?.Invoke(ex, thisValue) is var result && result!= null && result.Fixed)
                    return thisValue;
                throw ex;
            }
            return thisValue;
        }

        private static void CleanAndOrDelete(DirectoryInfo baseDirectory, bool recursive, bool deleteDir)
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

        public static DirectoryPath Delete(this DirectoryPath thisValue, string searchPattern= "*")
        {
            CleanAndOrDelete(new DirectoryInfo(thisValue), false, true);
            return thisValue;
        }

        public static DirectoryPath Clean(this DirectoryPath thisValue, string searchPattern = "*")
        {
            CleanAndOrDelete(new DirectoryInfo(thisValue), false, false);
            return thisValue;
        }

    }
}
