using System.Collections.Generic;
using System.Linq;
using EifelMono.Fluent.IO;
using EifelMono.Fluent.Extensions;
using System.Diagnostics;
using System;
using EifelMono.Fluent.Log;
using System.Threading.Tasks;
using EifelMono.Fluent.DotNet.Classes;

#pragma warning disable IDE1006 // Naming Styles
namespace EifelMono.Fluent
{
    public static partial class dotnet
    {
        public static class Scan
        {
            public static IEnumerable<DirectoryPath> Sdks
                => OS.sdks.GetDirectories("*").Where(d => d.SplitValuesLast.StartsWithDigit()).Select(d => d.MakeAbsolute());
            public static IEnumerable<string> SdkNames
                => Sdks.Select(d => d.SplitValuesLast);

            public static IEnumerable<string> SdkReleaseNames
                => SdkNames?.Where(name => !name.Contains("-"));
            public static IEnumerable<string> SdkBetaNames
                => SdkNames?.Where(name => name.Contains("-"));
            public static string SdkName
                => SdkNames?.LastOrDefault() ?? "";
            public static string SdkReleaseName
                => SdkReleaseNames?.LastOrDefault() ?? "";
            public static string SdkBetaName
                => SdkBetaNames?.LastOrDefault() ?? "";

            public static string MajorReleaseVersion => SdkReleaseNames?.LastOrDefault() ?? "";

            public static string MajorBetaVersion => SdkBetaNames?.LastOrDefault() ?? "";

            public static IEnumerable<DirectoryPath> Runtimes
                => OS.runtimes.GetDirectories("*/*").Where(d => d.SplitValuesLast.StartsWithDigit()).Select(d => d.MakeAbsolute());
        }
    }
}
