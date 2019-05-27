# EifelMono.Fluent  

## Infos
    
* [![Build Status](https://eifelmono.visualstudio.com/Eifelmono%20Fluent/_apis/build/status/Eifelmono%20Fluent-CI%20Ubuntu?branchName=master)](https://eifelmono.visualstudio.com/Eifelmono%20Fluent/_build/latest?definitionId=3&branchName=master) Ubuntu 
* [![Build Status](https://eifelmono.visualstudio.com/Eifelmono%20Fluent/_apis/build/status/Eifelmono%20Fluent-CI%20macOS?branchName=master)](https://eifelmono.visualstudio.com/Eifelmono%20Fluent/_build/latest?definitionId=4&branchName=master) macOS
* [![Build Status](https://eifelmono.visualstudio.com/Eifelmono%20Fluent/_apis/build/status/Eifelmono%20Fluent-CI%20Windows?branchName=master)](https://eifelmono.visualstudio.com/Eifelmono%20Fluent/_build/latest?definitionId=5&branchName=master) Windows
* [![NuGet][main-nuget-badge]][main-nuget] nuget 
* [fuget.org](https://www.fuget.org/packages/EifelMono.Fluent) doc 😉 


[main-nuget]: https://www.nuget.org/packages/EifelMono.Fluent/
[main-nuget-badge]: https://img.shields.io/nuget/v/EifelMono.Fluent.svg?style=flat-square&label=nuget

## Fluent handling for files and directories

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

* Visual Studio 2019 RC/preview
* dotnet core 3.0 Roslyn c# 8.0 

## Safe in the Name 

=> (bool Ok, T Value)

Methods for fluent
=> (bool Ok, T Value, TF FluentValue)

## Social
<a href="https://twitter.com/eifelmono"><img src="https://img.shields.io/badge/twitter-%40eifelmono-55acee.svg?style=flat-square"></a>
<a href="https://facebook.com/andreas.klapperich"><img src="https://img.shields.io/badge/facebook-andreas.klapperich-55acee.svg?style=flat-square"></a>
