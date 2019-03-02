using System;
using EifelMono.Fluent.IO;

namespace EifelMono.Fluent.ConsoleTestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"{args.Length}");
            Console.WriteLine($"fluent.App");
            Console.WriteLine(fluent.App.ToString());

            Console.WriteLine($"fluent.FluentLib");
            Console.WriteLine(fluent.FluentLib.ToString());

            Console.WriteLine(fluent.FluentLib.Version);

            var tempFile = FilePath.OS.Temp;
            Console.WriteLine(tempFile);

            tempFile.WriteLine($"{DateTime.Now}");
            tempFile.WriteLine(fluent.App.ToString());
            tempFile.WriteLine(fluent.FluentLib.ToString());

            Console.WriteLine(tempFile.ReadAllText());



        }
    }
}
