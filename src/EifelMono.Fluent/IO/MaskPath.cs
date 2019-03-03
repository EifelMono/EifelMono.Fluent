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

        protected static bool IsMaskAllOnly(List<string> searchMasks)
            => searchMasks.Count == 1 && searchMasks[0] == s_placeholderAll;

        protected bool IsMaskAll(List<string> searchMasks, int position)
            => searchMasks.Count > position && searchMasks[position] == s_placeholderAll;

        protected bool IsMaskMulti(List<string> searchMasks, int position)
            => searchMasks.Count > position && searchMasks[position] == s_placeholderMulti;
        protected bool IsMaskSingle(List<string> searchMasks, int position)
            => searchMasks.Count > position && searchMasks[position] == s_placeholderSingle;

        protected bool IsMaskMultiOrSingle(List<string> searchMasks, int position)
            => IsMaskMulti(searchMasks, position) || IsMaskSingle(searchMasks, position);

        // From http://www.codeproject.com/KB/recipes/wildcardtoregex.aspx:
        // So something like foo*.xls? will get transformed to ^foo.*\.xls.$.
        protected string WildcardToRegex(string pattern)
            => $"^{Regex.Escape(pattern).Replace(@"\*", ".*").Replace(@"\?", ".")}$";

        protected bool IsMaskMatch(List<string> searchMasks, int position, string directoryName)
        {
            if (position >= searchMasks.Count)
                return false;
            var match = new Regex(WildcardToRegex(searchMasks[position]));
            return match.IsMatch(directoryName);
        }

        protected bool IsMaskLast(List<string> searchMasks)
            => searchMasks.Count == 1;

        protected List<DirectoryPath> SearchDirectories(bool root, DirectoryPath startDirectory, List<string> searchMasks)
        {
            var result = new List<DirectoryPath>();
            if (root && IsMaskAllOnly(searchMasks))
                result.Add(startDirectory);

            var directories = Directory.GetDirectories(startDirectory).Select(d => new DirectoryPath(d)).ToList();
            foreach (var directory in directories)
            {
                var directoryName = directory.SplitValues.Last();
                if (searchMasks.Count > 0)
                {
                    if (IsMaskAll(searchMasks, 0))
                    {
                        if (searchMasks.Count == 1)
                        {
                            result.Add(directory);
                            result.AddRange(SearchDirectories(false, directory, searchMasks));
                        }
                        else
                        {
                            if (IsMaskMatch(searchMasks, 1, directoryName))
                            {
                                if (!IsMaskMultiOrSingle(searchMasks, 2))
                                    result.Add(directory);
                                result.AddRange(SearchDirectories(false, directory, searchMasks.Skip(2).ToList()));
                            }
                            else
                            {
                                result.AddRange(SearchDirectories(false, directory, searchMasks));
                            }
                        }
                    }
                    else
                    {
                        if (IsMaskMatch(searchMasks, 0, directoryName))
                        {
                            result.Add(directory);
                            result.AddRange(SearchDirectories(false, directory, searchMasks.Skip(1).ToList()));
                        }
                    }
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
            var matches = SplitValues.Last().Split(',').Select(s => new Regex(WildcardToRegex(s)));
            // multiples file mask seperate by ,
            // *.cs,*.h,*.md
#if NETSTANDARD2_0
            Parallel.ForEach(SearchDirectories(true, startDirectory, SplitValues.Take(SplitValues.Count() - 1).ToList()),
                searchDirectory =>
                {
                    var localFiles = Directory.GetFiles(searchDirectory, "*").Select(f => new FilePath(f)).ToList();
                    var matchFiles = new List<FilePath>();
                    foreach (var file in localFiles)
                        foreach (var match in matches)
                            if (match.IsMatch(file.FileName))
                            {
                                matchFiles.Add(file);
                                break;
                            }
                    lock (files)
                        files.AddRange(matchFiles);
                });
#else
            foreach (var searchDirectory in SearchDirectories(true, startDirectory, SplitValues.Take(SplitValues.Count() - 1).ToList()))
            {
                 var localFiles = Directory.GetFiles(searchDirectory, "*").Select(f => new FilePath(f)).ToList();
                    var matchFiles = new List<FilePath>();
                    foreach (var file in localFiles)
                        foreach (var match in matches)
                            if (match.IsMatch(file.FileName))
                            {
                                matchFiles.Add(file);
                                break;
                            }
                    lock (files)
                        files.AddRange(matchFiles);
            };
#endif
            return files;
        }
    }
}
