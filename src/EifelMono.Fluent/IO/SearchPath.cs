using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using EifelMono.Fluent.Extensions;

namespace EifelMono.Fluent.IO
{
    public class SearchPath : IDisposable
    {
        public void Dispose()
        {
        }

        protected async Task<List<DirectoryPath>> SearchDirectoryAsync(string directoryPath, List<string> searchMaskDirectories)
        {
            await Task.Delay(1);
            var result = new List<DirectoryPath>();

            return result;
        }
        /*
        From http://www.codeproject.com/KB/recipes/wildcardtoregex.aspx:

public static string WildcardToRegex(string pattern)
{
    return "^" + Regex.Escape(pattern)
                      .Replace(@"\*", ".*")
                      .Replace(@"\?", ".")
               + "$";
}
So something like foo*.xls? will get transformed to ^foo.*\.xls.$.
         */
        protected (List<string> searchMaskDirectories, string SearchMaskFiles) SplitSearchMask(string searchMask)
        {
            var splitSearchMask = searchMask.NormalizePath().SplitPath().ToList();
            return (splitSearchMask.Take(splitSearchMask.Count() - 1).ToList(), splitSearchMask.Last());
        }

        public async Task<List<FilePath>> GetFilesAsync(string directoryPath, string searchMask)
        {
            var splitSearchMask = searchMask.NormalizePath().SplitPath().ToList();
            var searchMaskDirectories = splitSearchMask.Take(splitSearchMask.Count() - 1).ToList();
            var searchMaskFiles = splitSearchMask.Last();
            var result = new List<FilePath>();
            Parallel.ForEach(await GetDirectoriesAsync(directoryPath, searchMaskDirectories.ToPath()), foundDirectoryPath =>
            {
                var files = Directory.GetFiles(foundDirectoryPath, searchMaskFiles).Select(f => new FilePath(f)).ToList();
                lock (result)
                    result.AddRange(files);
            });
            return result;
        }

        public async Task<List<DirectoryPath>> GetDirectoriesAsync(string directoryPath, string searchMask)
        {
            var searchMaskDirectories = searchMask.NormalizePath().SplitPath().ToList();
            return await SearchDirectoryAsync(directoryPath, searchMaskDirectories);
        }
    }
}
