#addin nuget:?package=EifelMono.Fluent&version=0.0.1

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");
var sln= new FilePath("EifelMono.Fluent.sln");
Task("Clean")
.Does(()=> {
    for (int i= 0;i< 5; i++) // Try it more times.....
    try {
        CleanDirectories($"./src/**/bin/{configuration}/**/**");
        return;
    }
    catch {}
});
Task("Restore")
.Does(() => {
    DotNetCoreRestore(sln.FullPath);
});
Task("Build")
.Does(() => {
    DotNetCoreBuild(sln.FullPath, new DotNetCoreBuildSettings{
        Configuration= configuration
    });
});
Task("CleanNuget")
.Does(()=> {
     EnsureDirectoryExists($"./nuget");
     CleanDirectories($"./nuget");
});
Task("CopyNuget")
.Does(()=> {
    EnsureDirectoryExists($"./nuget");
    foreach(var file in GetFiles($"./src/**/bin/{configuration}/*.nupkg"))
        CopyFile(file, $"./nuget/{file.GetFilename()}");
});
Task("Doc")
.Does(()=> {
        StartAndReturnProcess("generateomd", new ProcessSettings
            { Arguments = $"/source=./src" }
        );
});
Task("Default")
.IsDependentOn("Clean")
.IsDependentOn("Restore")
.IsDependentOn("Build")
.IsDependentOn("CleanNuget")
.IsDependentOn("CopyNuget")
.Does(() => {
});
Task("EifelMono")
.Does(()=> {
    var globaljson= new EifelMono.Fluent.FilePath("global.json");
    var src= new EifelMono.Fluent.DirectoryPath("./src");
    foreach(var d in src.GetFiles("**/*.cs"))
        Information(d);
});
RunTarget(target);
