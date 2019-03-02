using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EifelMono.Fluent;
using EifelMono.Fluent.IO;

namespace ConsoleApp_Shared
{
    public static class SharedMain
    {
        public static void Run()
        {
            FilePath f = new FilePath("x.text");
            f.WriteAllText($"Timestamp {DateTime.Now}{Environment.NewLine}");

            if (f.Exists)
                Console.WriteLine($"{f.FullPath} exists");
            else
                Console.WriteLine($"{f.FullPath} does exists");

            Console.WriteLine($"file content from {f.FullPath}");
            Console.WriteLine(f.ReadAllText());

            Console.WriteLine($"fluent.App");
            Console.WriteLine(fluent.App.ToString());

            Console.WriteLine($"fluent.FluentLib");
            Console.WriteLine(fluent.FluentLib.ToString());

            Console.ReadLine();
        }
    }
}
