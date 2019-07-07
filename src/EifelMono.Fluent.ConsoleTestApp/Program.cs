using System;
using System.Collections.Generic;
using System.Linq;
using EifelMono.Fluent.IO;
using EifelMono.Fluent.Extensions;
using System.Diagnostics;
using System.IO;
using EifelMono.Fluent.NuGet;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using EifelMono.Fluent.DotNet;
using EifelMono.Fluent.Cataloge;

namespace EifelMono.Fluent.ConsoleTestApp
{
    class Program
    {
        static void Test()
        {
            var o1 = new
            {
                Name = "Name.1",
                Items = new List<string>
                {
                    "Items1.1",
                    "Items1.2",
                    "Items1.3",
                    "Items1.4",
                },
                o2 = new
                {
                    Name = "name.2",
                    Items = new List<string>
                    {
                        "Items2.1",
                        "Items2.2",
                        "Items2.3",
                        "Items2.4",
                    },
                }
            };
            var cataloge = o1.ToCataloge();
            Console.WriteLine(cataloge);
        }
        static async Task Main()
        {
            Test();

            Console.WriteLine(fluent.App.BuildTimeStampUtc.ToLocalTime());
            Console.WriteLine(fluent.App.BuildMachineName);
            Console.WriteLine(fluent.App.BuildReleaseName);

            {
                var (Ok, Value) = await dotnet.Shell.VersionAsync();
                if (Ok)
                    Console.WriteLine($"version {Value}");
                Console.ReadLine();
            }
            {
                if (await dotnet.Shell.SdksAsync() is var result && result.Ok)
                    foreach (var item in result.Value)
                        Console.WriteLine($"{item.IsBeta} {item.Version} [{item.Directroy}]");
                Console.ReadLine();
            }
            {
                if (await dotnet.Shell.RuntimesAsync() is var result && result.Ok)
                    foreach (var item in result.Value)
                        Console.WriteLine($"{item.IsBeta} {item.Version} [{item.Directroy}]");
                Console.ReadLine();
            }

            var v = await nuget.org.GetLastPackageVersionAsync("dotnet-serve");
            if (v.Ok)
                Console.WriteLine($"Last={v.Value}");
            var l = await nuget.org.GetPackageVersionsAsync("dotnet-serve");
            if (l.Ok)
                Console.WriteLine(string.Join("\r\n", l.Value));
            Console.ReadLine();

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"EifelMono.Fluent FilePath, DirectoryPath, ....");
            Console.ForegroundColor = ConsoleColor.White;
            // var testFile = new FilePath(@"C:\temp\src", "test.txt")
            var testFile = new FilePath(@"C:\temp\src\test.txt")
                .EnsureDirectoryExist()
                .IfExists.Delete(); // if file exist
            testFile.WriteLine("Line 1");
            testFile.WriteLine("Line 2");

            Console.WriteLine(testFile.FullPath);
            Console.WriteLine(testFile.FileName);
            Console.WriteLine(testFile.FileNameWithoutExtension);
            Console.WriteLine(testFile.Extension);
            Console.WriteLine(testFile.DirectoryName);
            Console.WriteLine(testFile.CreationTime);
            Console.WriteLine(testFile.CreationTimeUtc);
            Console.WriteLine(testFile.LastAccessTime);
            Console.WriteLine(testFile.LastWriteTime);

            testFile.Copy("test.bak");

            Console.WriteLine(@"**\*.txt,*.bak");
            foreach (var file in testFile.Directory.GetFiles(@"**\*.txt,*.bak"))
                Console.WriteLine(file);

            var newDir = testFile.Directory.Clone("Hello", "World").EnsureExist();
            Console.WriteLine("**");
            foreach (var directory in testFile.Directory.GetDirectories("**"))
                Console.WriteLine(directory);

            Console.WriteLine(@"**\Hello\**");
            foreach (var directory in testFile.Directory.GetDirectories(@"**\Hello\**"))
                Console.WriteLine(directory);
            Console.WriteLine(@"**\Hello\*");
            foreach (var directory in testFile.Directory.GetDirectories(@"**\Hello\*"))
                Console.WriteLine(directory);

            testFile.Copy(newDir);
            Console.WriteLine(@"**\*.txt,*.bak");
            foreach (var file in testFile.Directory.GetFiles(@"**\*.txt,*.bak"))
                Console.WriteLine(file);

            Console.WriteLine(@"**\Hello\**\*.txt,*.bak");
            foreach (var file in testFile.Directory.GetFiles(@"**\Hello\**\*.txt,*.bak"))
                Console.WriteLine(file);

            var jsonFile = new FilePath(testFile.Directory, "test.json")
                .IfExists.Delete();

            var spezialFolders = fluent.Enum.Values<Environment.SpecialFolder>()
                .Select(f => DirectoryPath.OS.SpecialFolderPath(f)).ToList();

            jsonFile.WriteAllText(spezialFolders.ToJson());
            var spezialFoldersFromFile = jsonFile.ReadAllText().FromJson<List<FilePath>>();

            Console.WriteLine("Waiting for key return");
            Console.ReadLine();

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"About App, Lib, OS");
            Console.ForegroundColor = ConsoleColor.White;

            Console.WriteLine(fluent.OSInfo.ToJson());
            Console.WriteLine(fluent.FluentLib.ToJson());
            Console.WriteLine(fluent.FluentLib.CustomAttributesAsJson());

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"fluent.App info about this app");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(fluent.App.ToJson());
            Console.WriteLine(fluent.App.BuildTimeStampUtc.ToLocalTime());
            Console.WriteLine(fluent.App.BuildMachineName);
            Console.WriteLine(fluent.App.BuildReleaseName);

            Console.WriteLine("Waiting for key return");
            Console.ReadLine();
        }
    }
}
