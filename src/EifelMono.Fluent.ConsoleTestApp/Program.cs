using System;

namespace EifelMono.Fluent.ConsoleTestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"fluent.App");
            Console.WriteLine($"Executable {fluent.App.Executable}");
            Console.WriteLine($"AssemblyVersion {fluent.App.AssemblyVersion}");
            Console.WriteLine($"FileVersion {fluent.App.FileVersion}");
            Console.WriteLine($"Version {fluent.App.Version}");
            Console.WriteLine($"Company {fluent.App.Company}");
            Console.WriteLine($"Copyright {fluent.App.Copyright}");
            Console.WriteLine($"Title {fluent.App.Title}");
            Console.WriteLine($"Product {fluent.App.Product}");
            Console.WriteLine($"Configuration {fluent.App.Configuration}");
            Console.WriteLine($"Culture {fluent.App.Culture}");
            Console.WriteLine($"Description {fluent.App.Description}");
            Console.WriteLine($"Trademark {fluent.App.Trademark}");
            Console.WriteLine($"FrameworkName {fluent.App.FrameworkName}");
            Console.WriteLine($"FrameworkDisplayName {fluent.App.FrameworkDisplayName}");
        }
    }
}
