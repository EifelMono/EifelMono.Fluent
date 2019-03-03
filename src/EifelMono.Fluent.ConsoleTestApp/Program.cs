using System;
using System.Collections.Generic;
using System.Linq;
using EifelMono.Fluent.IO;
using EifelMono.Fluent.Extensions;

namespace EifelMono.Fluent.ConsoleTestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var testFile = new FilePath(@"C:\temp\src", "test.txt")
                .EnsureDirectoryExist()
                .DeleteIfExist(); // if file exist
            testFile.WriteLine("Line 1");
            testFile.WriteLine("Line 2");

            testFile.Copy("test.bak");

            foreach (var file in testFile.Directory.GetFiles(@"**\*.txt,*.bak"))
                Console.WriteLine(file);

            var newDir = testFile.Directory.Clone("Hello", "World").EnsureExist();
            foreach (var directory in testFile.Directory.GetDirectories(@"**"))
                Console.WriteLine(directory);

            testFile.Copy(newDir);
            foreach (var file in testFile.Directory.GetFiles(@"**\*.txt,*.bak"))
                Console.WriteLine(file);

            var jsonFile = new FilePath(testFile.Directory, "test.json")
                .DeleteIfExist();

            var spezialFolders = fluent.Enum.Values<Environment.SpecialFolder>()
                .Select(f=> DirectoryPath.OS.SpecialFolderPath(f)).ToList();

            jsonFile.WriteAllText(spezialFolders.ToJson());
            var spezialFoldersFromFile = jsonFile.ReadAllText().FromJson<List<FilePath>>();

            Console.WriteLine($"fluent.App");
            Console.WriteLine(fluent.App.AllInfos());

            Console.WriteLine($"fluent.FluentLib");
            Console.WriteLine(fluent.FluentLib.AllInfo());

            Console.WriteLine(fluent.FluentLib.Version);

            Console.WriteLine(fluent.FluentLib.AllInfos());
        }
    }
}
