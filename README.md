# EifelMono.Fluent

## Fluent writing for file and directory operations

* [FilePath](https://github.com/EifelMono/EifelMono.Fluent/wiki/FilePath) file operations
* [DirectoryPath](https://github.com/EifelMono/EifelMono.Fluent/wiki/DirectoryPath) directory operations
* [fluent](https://github.com/EifelMono/EifelMono.Fluent/wiki/fluent) App, Lib, OS, ...
* [Extensions](https://github.com/EifelMono/EifelMono.Fluent/wiki/Extensions) generics, string, ...

```csharp
var testFile = new FilePath(@"C:\temp\src\test.txt")
    .EnsureDirectoryExist()
    .DeleteIfExist(); // if file exist
testFile.WriteLine("Line 1");
testFile.WriteLine("Line 2");

testFile.Copy("test.bak");

Console.WriteLine(testFile.FullPath);
Console.WriteLine(testFile.FileName);
Console.WriteLine(testFile.FileNameWithoutExtension);
Console.WriteLine(testFile.Extension);
Console.WriteLine(testFile.DirectoryName);
Console.WriteLine(testFile.CreationTime);
Console.WriteLine(testFile.CreationTimeUtc);
Console.WriteLine(testFile.LastAccessTime);
Console.WriteLine(testFile.LastWriteTime);

foreach (var file in testFile.Directory.GetFiles(@"**\*.txt,*.bak"))
    Console.WriteLine(file);
```
for more see the
* [wiki](https://github.com/EifelMono/EifelMono.Fluent/wiki)
* [Sample](https://github.com/EifelMono/EifelMono.Fluent/blob/master/src/EifelMono.Fluent.ConsoleTestApp/Program.cs)
* [Tests](https://github.com/EifelMono/EifelMono.Fluent/tree/master/src/EifelMono.Fluent.Test)

##  Requirements

* Visual Studio 2019 preview
*  

