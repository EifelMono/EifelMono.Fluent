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
        protected async Task<List<DirectoryPath>> SearchDirectoryAsync(DirectoryPath directory, List<string> searchMaskDirectories)
        {
            await Task.Delay(1);
            var directories = Directory.GetDirectories(directory).Select(d => new DirectoryPath(d)).ToList();
            if (searchMaskDirectories.Count == 0)
                return directories;
            else
                return new List<DirectoryPath>();
        }

        public static string s_PlaceholderSingle = "?";
        public static string s_PlaceholderMulti = "*";
        public static string s_PlaceholderGroup = "**";

        public (bool Ok, List<string> SearchMasks) MaskToMaskList(string searchMask)
        {
            searchMask = searchMask ?? "";
            var SearchMasks = searchMask.NormalizePath().SplitPath().ToList();
            if (SearchMasks.Count == 0)
                return (false, new List<string>());
            var lastPlacehoderGroup = false;
            foreach (var splitMask in SearchMasks)
            {
                if (splitMask.Contains(s_PlaceholderGroup) && splitMask.Length > s_PlaceholderGroup.Length)
                    return (false, new List<string>());
                if (splitMask == s_PlaceholderGroup)
                {
                    if (lastPlacehoderGroup)
                        return (false, new List<string>());
                    lastPlacehoderGroup = true;
                }
                else
                    lastPlacehoderGroup = false;
            }
            return (true, SearchMasks);
        }

        public async Task<List<DirectoryPath>> GetDirectoriesAsync(DirectoryPath directory, string searchMask)
        {
            if (MaskToMaskList(searchMask) is var result && result.Ok)
                return await SearchDirectoryAsync(directory, result.SearchMasks);
            else
                return new List<DirectoryPath>();
        }

        public async Task<List<FilePath>> GetFilesAsync(DirectoryPath directory, string searchMask)
        {
            var files = new List<FilePath>();
            if (MaskToMaskList(searchMask) is var result && result.Ok)
            {
                var searchMaskFiles = result.SearchMasks.Last();
                Parallel.ForEach(await SearchDirectoryAsync(directory, result.SearchMasks.Take(result.SearchMasks.Count() - 1).ToList()),
                    foundDirectory =>
                    {
                        var localFiles = Directory.GetFiles(foundDirectory, searchMaskFiles).Select(f => new FilePath(f)).ToList();
                        lock (files)
                            files.AddRange(localFiles);
                    });
            }
            return files;
        }
    }
}
