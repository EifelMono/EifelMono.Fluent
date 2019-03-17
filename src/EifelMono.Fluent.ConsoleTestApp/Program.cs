using System;
using System.Collections.Generic;
using System.Linq;
using EifelMono.Fluent.IO;
using EifelMono.Fluent.Extensions;

namespace EifelMono.Fluent.ConsoleTestApp
{
    class Program
    {
        static void Main()
        {
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
                .Select(f=> DirectoryPath.OS.SpecialFolderPath(f)).ToList();

            jsonFile.WriteAllText(spezialFolders.ToJson());
            var spezialFoldersFromFile = jsonFile.ReadAllText().FromJson<List<FilePath>>();

            Console.WriteLine("Waiting for key return");
            Console.ReadLine();

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"About App, Lib, OS");
            Console.ForegroundColor = ConsoleColor.White;

            Console.WriteLine(fluent.OS.ToJson());
            Console.WriteLine(fluent.FluentLib.ToJson());
            Console.WriteLine(fluent.FluentLib.CustomAttributesAsJson());

            Console.ForegroundColor= ConsoleColor.Yellow;
            Console.WriteLine($"fluent.App info about this app");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(fluent.App.ToJson());
            Console.WriteLine(fluent.App.BuildTimeStampUtc.ToLocalTime());
            Console.WriteLine(fluent.App.BuildMachineName);

            Console.WriteLine("Waiting for key return");
            Console.ReadLine();
        }
    }
}
