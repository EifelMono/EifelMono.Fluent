var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");
var sln= new FilePath("EifelMono.Fluent.sln");
var testcsproj= new FilePath("./src/EifelMono.Fluent.Test/EifelMono.Fluent.Test.csproj");
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
Task("CleanArtifacts")
.Does(()=> {
     EnsureDirectoryExists($"./artifacts");
     CleanDirectories($"./artifacts");
});
Task("CopyArtifacts")
.Does(()=> {
    EnsureDirectoryExists($"./artifacts");
    foreach(var file in GetFiles($"./src/**/bin/{configuration}/*.nupkg"))
        CopyFile(file, $"./artifacts/{file.GetFilename()}");
});
Task("Doc")
.Does(()=> {
        StartAndReturnProcess("generateomd", new ProcessSettings
            { Arguments = $"/source=./src" }
        );
});

Task("Test")
.Does(() => {
    Information(sln);
    StartProcess("dotnet", "tool install dotnet-dlla -g");
    DotNetCoreTest(sln.FullPath, new DotNetCoreTestSettings {
                        Configuration= configuration
    });
});

Task("Default")
.IsDependentOn("Clean")
.IsDependentOn("Restore")
.IsDependentOn("Build")
.IsDependentOn("CleanArtifacts")
.IsDependentOn("CopyArtifacts")
.Does(() => {
});

RunTarget(target);
