using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            foreach (var directory in await GetDirectoriesAsync(directoryPath, searchMaskDirectories.ToPath()))
            {

            }
            await Task.Delay(1);
            return result;
        }

        public async Task<List<DirectoryPath>> GetDirectoriesAsync(string directoryPath, string searchMask)
        {
            var searchMaskDirectories = searchMask.NormalizePath().SplitPath().ToList();
            return await SearchDirectoryAsync(directoryPath, searchMaskDirectories);
        }
    }
}
