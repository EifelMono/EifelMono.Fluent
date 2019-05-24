#if ! NETSTANDARD1_6
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using EifelMono.Fluent.DotNet.Classes;
using EifelMono.Fluent.Extensions;
using EifelMono.Fluent.IO;
using EifelMono.Fluent.Log;

namespace EifelMono.Fluent
{
#pragma warning disable IDE1006 // Naming Styles
    public static partial class dotnet
    {
        public static class Shell
        {
            public static async Task<(bool Ok, string Value)> RunAsync(string arguments, TimeSpan timespan)
            {
                try
                {
                    using var dotnetProcess = new Process();
                    dotnetProcess.StartInfo.FileName = "dotnet";
                    dotnetProcess.StartInfo.Arguments = arguments;
                    dotnetProcess.StartInfo.UseShellExecute = false;
                    dotnetProcess.StartInfo.RedirectStandardOutput = true;
                    dotnetProcess.Start();
                    var result = dotnetProcess.StandardOutput.ReadToEnd();
                    await Task.Delay(1);
                    dotnetProcess.WaitForExit((int)timespan.TotalMilliseconds);
                    return (!string.IsNullOrEmpty(result), result);
                }
                catch (Exception ex)
                {
                    ex.LogException();
                    return (false, null);
                }
            }
            public static Task<(bool Ok, string Value)> ExecuteAsync(string arguments)
                => RunAsync(arguments, TimeSpan.FromSeconds(10));

            public static Task<(bool Ok, string Value)> VersionAsync()
                => ExecuteAsync("--version");

            public class DotNetVersionItem
            {
                public string Version { get; set; }
                public bool IsBeta { get; set; }

                public string Directroy { get; set; }

                public override string ToString()
                    => $"{IsBeta} {Version} [{Directroy}]";
            }

            private static List<DotNetVersionItem> ConvertToDotNetVersionItem(string value)
            {
                var result = new List<DotNetVersionItem>();
                foreach (var line in value.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries))
                {
                    var split = line.Split('[');
                    if (split.Length > 1)
                    {
                        var version = split[0].Trim();
                        result.Add(new DotNetVersionItem
                        {
                            Version = version,
                            IsBeta = version.Contains("-"),
                            Directroy = split[1].Replace("]", "").Trim()
                        });
                    }
                }
                return result;
            }
            public static async Task<(bool Ok, List<DotNetVersionItem> Value)> SdksAsync()
            {
                if (await ExecuteAsync("--list-sdks") is var result && result.Ok)
                    return (true, ConvertToDotNetVersionItem(result.Value));
                return (false, null);
            }

            public static async Task<(bool Ok, List<DotNetVersionItem> Value)> RuntimesAsync()
            {
                if (await ExecuteAsync("--list-runtimes") is var result && result.Ok)
                    return (true, ConvertToDotNetVersionItem(result.Value));
                return (false, null);
            }


            public class DotNetToolItem
            {
                public string PackageId { get; set; }
                public string Version { get; set; }
                public bool IsBeta { get; set; }

                public string Command { get; set; }

                public override string ToString()
                    => $"{PackageId} {IsBeta} {Version} [{Command}]";
            }

            private static List<DotNetToolItem> ConvertToDotNetToolItem(string value)
            {
                var result = new List<DotNetToolItem>();
                var index = 0;
                foreach (var line in value.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries))
                {
                    if (index++ < 2)
                        continue;
                    var split = line.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                    if (split.Length > 2)
                    {
                        var version = split[1].Trim();
                        result.Add(new DotNetToolItem
                        {
                            PackageId = split[0].Trim(),
                            Version = version,
                            IsBeta = version.Contains("-"),
                            Command = split[2].Trim()
                        });
                    }
                }
                return result;
            }
            public static async Task<(bool Ok, List<DotNetToolItem> Value)> ToolsAsync(string toolPath = "-g")
            {
                if (await ExecuteAsync($"tool list {toolPath}") is var result && result.Ok)
                    return (true, ConvertToDotNetToolItem(result.Value));
                return (false, null);
            }
        }
    }
#pragma warning restore IDE1006 // Naming Styles
}
#endif
