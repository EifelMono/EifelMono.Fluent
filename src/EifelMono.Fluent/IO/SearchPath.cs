using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace EifelMono.Fluent.IO
{
    public class SearchPath : IDisposable
    {
        public void Dispose()
        {
        }
        public async Task<List<FilePath>> GetFilesAsync(string directoryPath, string searchMask)
        {
            var result = new List<FilePath>();
            await Task.Delay(1);
            return result;
        }

        public async Task<List<DirectoryPath>> GetDirectoriesAsync(string directoryPath, string searchMask)
        {
            var result = new List<DirectoryPath>();
            await Task.Delay(1);
            return result;
        }

    }
}
