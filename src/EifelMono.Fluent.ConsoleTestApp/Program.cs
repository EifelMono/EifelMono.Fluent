using System;

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
        }
    }
}
