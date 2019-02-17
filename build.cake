///////////////////////////////////////////////////////////////////////////////
// ARGUMENTS
///////////////////////////////////////////////////////////////////////////////

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
     CleanDirectories($"./nuget");
});

Task("CopyNuget")
.Does(()=> {
    foreach(var file in GetFiles($"./src/**/bin/{configuration}/*.nupkg"))
        CopyFile(file, $"./nuget/{file.GetFilename()}");
});

Task("Default")
.IsDependentOn("Clean")
.IsDependentOn("Restore")
.IsDependentOn("Build")
.IsDependentOn("CleanNuget")
.IsDependentOn("CopyNuget")
.Does(() => {
});


Task("DirTest")
.Does(() => {
    foreach(var file in GetFiles("./src/**"))
        Information(file);
});

RunTarget(target);
