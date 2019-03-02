using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using EifelMono.Fluent.Extensions;

namespace EifelMono.Fluent.IO
{
    public class MaskPath : ValuePath
    {
        public MaskPath(string value)
        {
            Value = value;
        }

        public static string s_placeholderSingle = "?";
        public static string s_placeholderMulti = "*";
        public static string s_placeholderAll = "**";

        protected override bool CheckValue()
        {
            if (SplitValues.Count == 0)
                return false;
            var lastAll = false;
            foreach (var splitValue in SplitValues)
            {
                if (splitValue.Contains(s_placeholderAll) && splitValue.Length > s_placeholderAll.Length)
                    return false;
                if (splitValue == s_placeholderAll)
                {
                    if (lastAll)
                        return false;
                    lastAll = true;
                }
                else
                    lastAll = false;
            }
            return true;
        }

        /*
       From http://www.codeproject.com/KB/recipes/wildcardtoregex.aspx:
       */

        // So something like foo*.xls? will get transformed to ^foo.*\.xls.$.
        public string WildcardToRegex(string pattern)
            => "^" + Regex.Escape(pattern)
                        .Replace(@"\*", ".*")
                        .Replace(@"\?", ".")
                   + "$";

        protected bool IsMasksAll(List<string> searchMasks)
            => searchMasks.Count == 1 && searchMasks[0] == s_placeholderAll;
        protected bool IsMasksText(List<string> searchMasks)
            => !IsMasksAll(searchMasks);

        protected bool IsMaskAll(List<string> searchMasks)
            => searchMasks.Count > 0 && searchMasks[0] == s_placeholderAll;

        protected bool IsMaskAllAndText(List<string> searchMasks, string directory)
        {
            if (IsMaskAll(searchMasks))
                if (IsMaskText(searchMasks.Skip(1).ToList(), directory))
                    return true;
            return false;

        }
        protected bool IsMaskText(List<string> searchMasks, string directory)
        {
            if (searchMasks.Count == 0)
                return false;
            var match = new Regex(WildcardToRegex(searchMasks[0]));
            if (match.IsMatch(directory))
                return true;
            return false;
        }


        protected List<DirectoryPath> SearchDirectories(bool root, DirectoryPath startDirectory, List<string> searchMasks)
        {
            var result = new List<DirectoryPath>();
            if (root && IsMasksAll(searchMasks))
                result.Add(startDirectory);

            var directories = Directory.GetDirectories(startDirectory).Select(d => new DirectoryPath(d)).ToList();
            foreach (var directory in directories)
            {
                var directoryName = directory.SplitValues.Last();
                if (IsMaskAllAndText(searchMasks, directoryName))
                {
                    result.Add(directory);
                    result.AddRange(SearchDirectories(false, directory, searchMasks.Skip(2).ToList()));
                }
                else if (IsMaskAll(searchMasks))
                {
                    result.Add(directory);
                    result.AddRange(SearchDirectories(false, directory, searchMasks));
                }
                else if (IsMaskText(searchMasks, directoryName))
                {
                    result.Add(directory);
                    result.AddRange(SearchDirectories(false, directory, searchMasks.Skip(1).ToList()));
                }

            }
            return result;
        }

        public async Task<List<DirectoryPath>> GetDirectoriesAsync(DirectoryPath directory)
        {
            await Task.Delay(1);
            var startDirectory = directory.Clone().MakeAbsolute();
            if (!startDirectory.Exists)
                return new List<DirectoryPath>();
            if (!Ok)
                return new List<DirectoryPath>();
            return SearchDirectories(true, startDirectory, SplitValues);
        }

        public async Task<List<FilePath>> GetFilesAsync(DirectoryPath directory)
        {
            await Task.Delay(1);
            var startDirectory = directory.Clone().MakeAbsolute();
            if (!Ok)
                return new List<FilePath>();
            var files = new List<FilePath>();
            var fileMask = SplitValues.Last();
            // multiples file mask seperate by ,
            // *.cs,*.h,*.md
#if NETSTANDARD2_0
            Parallel.ForEach(SearchDirectories(true, startDirectory, SplitValues.Take(SplitValues.Count() - 1).ToList()),
                searchDirectory =>
                {
                    var localFiles = Directory.GetFiles(directory, fileMask).Select(f => new FilePath(f)).ToList();
                    lock (files)
                        files.AddRange(localFiles);
                });
#else
            foreach(var searchDirectory in SearchDirectories(true, startDirectory, SplitValues.Take(SplitValues.Count() - 1).ToList()))
            {
                var localFiles = Directory.GetFiles(directory, fileMask).Select(f => new FilePath(f)).ToList();
                lock (files)
                    files.AddRange(localFiles);
            };
#endif
            return files;
        }
    }
}
